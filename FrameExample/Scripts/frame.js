
function ClickFrameElement(path)
{
    SelectFramePath(path).click();
}

function SelectFramePath(path)
{
    return $('[frame-path="' + path + '"]');
}

function HandleReplace(response) {
    SelectFramePath(response.path).replaceWith(response.html);
}

function HandleRemove(response) {
    SelectFramePath(response.path).remove();
}

function HandleAdd(response) {
    SelectFramePath(response.path).append(response.html);
}

function HandleScript(response) {
    eval(response.script);
}

function HandleEventResponse(response) {
    switch(response.name)
    {
        case "script":
            HandleScript(response);
            break;
        case "replace":
            HandleReplace(response);
            break;
        case "remove":
            HandleRemove(response);
            break;
        case "add":
            HandleAdd(response);
            break;
    }
}

function HandleEventResponses(data) {

    for (var i = 0; i < data.d.length; i++)
        HandleEventResponse(data.d[i]);
}

function FrameEvent(path, vars) {

    var postData = [];
    if (vars != undefined)
        postData = postData.concat(vars);

    postData.push({ name: "path", value: path });
    postData.push({ name: "mouse-x", value: event.clientX });
    postData.push({ name: "mouse-y", value: event.clientY });

    $("[frame-input='true']").each(function () {
        postData.push({ name: 'frame-input-' + $(this).attr('frame-path'), value: $(this).val() });
    });

    $.ajax(
    {
        type: "post",
        data: postData,
        url: "/Frame/Event",
        dataType: "json",
        timeout: 0,
        cache: false,
        success: function (data) {
            HandleEventResponses(data);
        },
        error: function (objAJAXRequest, error) {
            $('body').append('<div style="position: absolute; width: 450px; color: Red; border: thick solid Red; padding: 20px; left: 10px; top: 10px; background-color: White;"><center>This session has expired.</center></div>');
        }
    });
}