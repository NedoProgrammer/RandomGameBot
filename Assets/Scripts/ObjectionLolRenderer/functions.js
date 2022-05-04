const delay = ms => new Promise(resolve => setTimeout(resolve, ms));

window.Share = async () => {
    $(".v-btn__content:contains('Submit')")[0].click();
};

window.Share2 = async () => {
    await delay(30000);
    $(".v-btn__content:contains('Submit')")[1].click();
    await delay(1000);
};

window.GetLink = async () => {
    $(".v-btn__content:contains('Share')")[0].click();
    await delay(500);
    return $(".v-text-field__slot").children()[0].value;
};

window.OpenImageMenu = () => {
    $("i[data-v-20aa1538='']")[0].click();
}

window.GetNameInput = () => {
    return $($($($("p[data-v-20aa1538]").parent().children()[1]).children().children()[0]).children()[1]).children()[1];
}

window.GetUrlInput = () => {
  return $($($($("p[data-v-20aa1538]").parent().children()[2]).children().children()[0]).children()[1]).children()[1];  
};

window.FinishImage = async () => {
    $($($($("p[data-v-20aa1538]").parent().children()[3])).children().children().children()[0]).children()[1].click();
    await delay(1000);
    $("p[data-v-20aa1538]").parent().children()[4].click();
};