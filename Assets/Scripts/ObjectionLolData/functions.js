function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms))
}


window.CreateFrame = function()
{
    $(".v-icon.notranslate.v-icon--left.mdi.mdi-plus.theme--dark")[0].click()
}

window.OpenFirstFrame = function()
{
    try {
        $(".pointer-item")[0].click();
    }
    catch (e) {}
}

window.OpenCharacterList = function()
{
    $(".mb-2.mb-sm-4").children().children().children().children().children()[0].click()
    $(".mb-2.mb-sm-4").children().children().children().children().children()[0].click()
}

window.OpenEmotionList = function()
{
    $(".mb-4").not(".mb-sm-0").children().children().children().click()
    $(".mb-4").not(".mb-sm-0").children().children().children().click()
}

window.GetCharacters = async function()
{
    OpenFirstFrame();
    await sleep(250);
    OpenCharacterList();
    await sleep(250);

    const list = $(".v-menu__content.theme--dark.v-menu__content--fixed.menuable__content__active.v-autocomplete__content")
    while (list[0].scrollHeight < 4401)
    {
        const scrollHeight = list[0].scrollHeight
        list.scrollTop(scrollHeight)
        await sleep(250);
    }
    
    const arr = [];
    $("[id*='list-item-']").each((i, obj) => {
        arr.push({name: obj.innerText, id: obj.id, raw: obj, emotions: [], formattedName: obj.innerText.replaceAll(/[^\w\s]|_/g, "").replaceAll(/\s+/g, " ").trim().split(" ").map(x => x.trim()[0].toUpperCase() + x.trim().slice(1)).join("")});
    });
    arr.splice(0, 2);
    return arr;
}

window.GetEmotion = async function()
{
    const filtered = window.CharacterInfo.map(x => x.name);
    OpenEmotionList();
    const arr = [];
    $("[id*='list-item-']").each((i, obj) => {
        if(filtered.indexOf(obj.innerText) === -1)
            arr.push({name: obj.innerText, id: obj.id, raw: obj, formattedName: obj.innerText.replaceAll(/[^\w\s]|_/g, "").replaceAll(/\s+/g, " ").trim().split(" ").map(x => x.trim()[0].toUpperCase() + x.trim().slice(1)).join("")});
    });
    arr.splice(0, 2);
	console.log(arr);
    if(window.CharacterInfo[window.CurrentIndex] !== undefined)
        window.CharacterInfo[window.CurrentIndex].emotions = arr;
}

window.CloseCharacterMenu = function()
{
    $(".v-btn__content:contains('Close')")[1].click();
}

window.DeleteFirstFrame = function()
{
    $(".v-icon.notranslate.mdi.mdi-delete.theme--dark")[0].click();
}

window.Save = async function()
{
    $('.v-btn__content:contains("Project")')[0].click();
    await sleep(100);
    $(".v-list-item__title:contains('Save')")[0].click();
    await sleep(100);
    $('.v-btn__content:contains("Save")')[1].click();
    await sleep(100);
}