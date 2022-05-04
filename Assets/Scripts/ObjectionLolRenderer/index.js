const puppeteer = require('puppeteer');
const fs = require('fs');
const path = require('path');
const alert = require("alert");
const crypto = require("crypto");
const { launch, getStream }  = require("puppeteer-stream");
const delay = ms => new Promise(resolve => setTimeout(resolve, ms));

(async () => {
    console.clear();
    if(!fs.existsSync("scene.objection"))
    {
        console.log("scene.objection file not found!");
        process.exit(1);
    }
    if(!fs.existsSync("additional.json"))
    {
        console.log("additional.json file not found!");
        process.exit(1);
    }
    
    if(fs.existsSync("objection.webm"))
        fs.rmSync("objection.webm");

    console.log("loading additional information");
    const additional = JSON.parse(fs.readFileSync("additional.json").toString());
    
    console.log("opening chromium");

    const browser = await launch({args: ['--start-maximized', '--disable-web-security'], headless: false});
    const page = await browser.newPage();

    console.log("opening objection.lol");
    await page.goto('https://objection.lol/maker');
    await page.setViewport({
        width: 1920,
        height: 1080,
        deviceScaleFactor: 3
    });

    console.log("injecting scripts");
    await page.addScriptTag({url: "https://code.jquery.com/jquery-3.6.0.min.js"});
    await page.addScriptTag({path: "functions.js"});
    console.log("injected");
    
    await delay(500);
    
    console.log("--> step 1: uploading scene");
    const handle = await page.$("input[type='file']");
    await handle.uploadFile("scene.objection");
    
    console.log("--> step 2: applying additional information");
    for(const evidenceTag of additional.evidence)
    {
        await delay(250);
        
        const textarea = await page.evaluate((i) => {
            const list = $(".frameTextarea")[i].classList;
            return list[list.length - 1];
        }, evidenceTag.index);
        await page.click(`.${textarea}`);
        console.log(`.${textarea}`);
        
        await delay(500);
        
        let input = await page.evaluate(async () => {
            await delay(500);
            window.OpenImageMenu();
            await delay(500);
            return window.GetNameInput().id;
        }, evidenceTag.index, evidenceTag.url);
        await page.type(`#${input}`, crypto.randomBytes(10).toString('hex'));
        
        input = await page.evaluate(() => window.GetUrlInput().id);
        await page.type(`#${input}`, evidenceTag.url);
        
        await page.evaluate(async () => {
            await window.FinishImage();
            await delay(500);
            window.OpenImageMenu();
        });
    }
    
    await delay(1000);
    
    console.log("--> step 3: sharing scene");
    await page.evaluate(async () => await window.Share());
    await alert("Press the captcha!");
    await page.evaluate(async () => await window.Share2());
    
    console.log("--> step 3.5: getting link");
    const link = await page.evaluate(async () => await window.GetLink());
    const id = link.split("/").pop();
    
    console.log("--> step 4: rendering scene");
    await page.goto(`https://objection.lol/record/${id}`);
    await page.setViewport({
        width: 960,
        height: 640
    })
    const file = fs.createWriteStream(__dirname + "/objection.webm");
    const stream = await getStream(page, { audio: true, video: true });
    stream.pipe(file);
    await page.on('console', async event => {
       if(event.text() === "LOADED")
       {
           await page.evaluate(() => document.getElementsByClassName("court-bg")[0].click());
       }
       else if(event.text() === "DONE")
       {
           await delay(5000);
           await stream.destroy();
           file.close();
		   await browser.close();
       }
    });
})();