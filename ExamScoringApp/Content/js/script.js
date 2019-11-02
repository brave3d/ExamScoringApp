document.addEventListener('mousedown', onMouseDown);
document.addEventListener('mouseup', onMouseUp);
var temp = document.querySelector('#shareBoxTemplate');

function onMouseDown() {
    document.getSelection().removeAllRanges();
    var shareBox = document.querySelector('#shareBox');
    if (shareBox !== null)
        shareBox.remove();
}

function onMouseUp() {
    var sel = document.getSelection(),
        txt = sel.toString();
    if (txt !== "") {
        var range = sel.getRangeAt(0);
        if (range.startContainer.parentElement.parentElement.localName === "article" || range.startContainer.parentElement.localName === "article") {
            // document.body.insertBefore(document.importNode(temp.content, true), temp);
            document.getElementById("container").insertBefore(document.importNode(temp.content, true), temp);
            // document.getElementById("container").appendChild(newNode);

            var rect = range.getBoundingClientRect();
            var shareBox = document.querySelector('#shareBox');
            //shareBox.style.top = `calc(${rect.top}px - 38px)`;
            //shareBox.style.left = `calc(${rect.left}px + calc(${rect.width}px / 2) - 288px)`;
            var shareBtn = shareBox.querySelector('button');
            shareBtn['shareTxt'] = txt;
            console.log(txt);

            shareBtn.addEventListener('mousedown', makeBlank, true);
        }
    }
}

blanks = [];
let getNodes = str => new DOMParser().parseFromString(str, 'text/html').body.childNodes;
let randomId = () => '_' + Math.random().toString(36).substr(2, 9);

function insertAfter(referenceNode, newNode) {
    referenceNode.parentNode.insertBefore(newNode, referenceNode.nextSibling);
}

function makeBlank() {
    this.remove();
    questionTxt = document.getElementById('question').innerText;
    var index = questionTxt.indexOf(this.shareTxt);
    var bId = randomId();
    document.getElementById('blanks').appendChild(getNodes(
        ` <div class="blank" id="${bId}">
	<label> Answer <input class="form-control" type="text" disabled="disabled" value="${this.shareTxt}"></label>
	<label> Score  <input class="form-control" type="number" value="10" max="10" min="0" placeholder="score"></label>
	<label> Index  <input class="form-control" type="number" value="${index}" placeholder="index" disabled="disabled"></label>
	<button type="button" class="btn btn-default" onclick="copy('${bId}')">Clone</button>
	 </div>`)[0]);
    highlight();
    document.getSelection().removeAllRanges()
}

function copy(blankId) {
    blank = document.getElementById(blankId);
    var copy = blank.cloneNode(true);
    var copyId = randomId();
    copy.setAttribute('id', `${copyId}`);
    console.log(copy);
    copy.children[3].setAttribute("onclick", `copy('${copyId}')`);
    copy.children[0].children[0].removeAttribute("disabled");

    insertAfter(blank, copy);
}




var highlighter;

rangy.init();
highlighter = rangy.createHighlighter();
highlighter.addClassApplier(rangy.createClassApplier('highlight'));

function highlight() {
    highlighter.highlightSelection('highlight');
    var selTxt = rangy.getSelection();
    console.log('selTxt: ' + selTxt);
    //   rangy.getSelection().removeAllRanges();
}

function removeHighlights() {
    highlighter.removeAllHighlights();
}

function reset() {
    removeHighlights();
    blanks = [];
    document.getElementById('blanks').innerHTML = "";
    $('#question').html($('#Text').val());
}
// document.getElementById('reset').addEventListener('click', removeHighlights);




$("#Text").on('input', (e) => {
    $('#question').html(e.target.value);
    reset();
});


var urlPerfix = "/ExamScoring"
function save() {
    var blanks = [];
    var points = document.getElementById('Points').value;
    var examId = $('#exams-select').find(":selected").attr('data-id');
    BlanksArr = Array.from(document.getElementsByClassName('blank'));
    BlanksArr.forEach(b => blanks.push(
        {
            "AnswerTxt": b.children[0].children[0].value,
            "Score": b.children[1].children[0].value,
            "Index": b.children[2].children[0].value,
        }));
    console.log(blanks);
    const groupedBlanks = _.groupBy(blanks, b => b.Index);
    const finalBlanks = [];
    Object.entries(groupedBlanks).forEach(e => { finalBlanks.push({ "Index": e[0], "PossibleAnswers": e[1] }) })

    //console.log(groupedBlanks);
    //console.log(finalBlanks);
    var text = $('#Text').val();
    var dbQuestionId = null;
    var requestExamId = null;
    if (document.location.pathname.indexOf('Edit')>0) {
        var pathChunks = document.location.pathname.split('/');
        dbQuestionId = pathChunks[pathChunks.length - 1];
       
        if (document.location.search.split("=")[1] !=undefined) {
            requestExamId = document.location.search.split("=")[1];
        }
    }
    data = {
        "examId": examId,
        "text": text,
        "blanks": finalBlanks,
        "points": points,
        "questionId": dbQuestionId
    }

    $.ajax({
        url: urlPerfix + '/Questions/SaveQuestion',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            //alert();
            if (response.success)
                advAlert("Success!", response.responseText, "success", requestExamId)
            else
                advAlert("Error!", response.responseText, "error")
        },
        error: function () {
            advAlert("Error!", "Try again later", "error")
        }
    });

}

function advAlert(title, text, type, requestExamId=null) {
    swal(title, text, type).then((value) => {
        if (type == "success") {
            if (requestExamId != null)
                window.location.href = window.location.origin + urlPerfix + `/Questions/Index?examId=${requestExamId}`;
            else
                window.location.href = window.location.origin + urlPerfix+  `/Questions/Index`;
        }

    });
}
