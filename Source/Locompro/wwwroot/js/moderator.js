function selectReport(index) {
    let modalId = "#ReportsPerSubmissionModal" + index;
    $(modalId).modal('show');
}

function reportAction(reportActions, userId, entryTime) {
    let url = window.location.pathname;
    url += "?handler=ActOnReport";
    
    console.log(reportActions);
    
    console.log(">>" + entryTime);

    let dataToSend = {
        Action: "" + reportActions,
        SubmissionUserId: userId,
        SubmissionEntryTime: entryTime
    };

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(dataToSend)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
        });
}

function changeModeratorPage(element) {
    let page = element.getValue();
    
    let url = window.location.pathname;
    url + "?pageIndex=" + page;
    
    window.location.href = url;
}