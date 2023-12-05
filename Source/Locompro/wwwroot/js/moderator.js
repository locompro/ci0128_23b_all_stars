function selectReport(index) {
    let modalId = "#ReportsPerSubmissionModal" + index;
    $(modalId).modal('show');
}

function selectAutoReport(index) {
    let modalId = "#AutoReportsPerSubmissionModal" + index;
    $(modalId).modal('show');
}

function reportAction(reportActions, userId, entryTime) {
    let url = window.location.pathname;
    url += "?handler=ActOnReport";

    let dataToSend = {
        Action: reportActions,
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
            location.href = location.href;
        });
}

function changeModeratorPage(element) {
    let page = element.getValue();
    
    let url = window.location.pathname;
    url += "?pageIndex=" + page;
    
    window.location.href = url;
}

document.addEventListener("DOMContentLoaded", function () {
    // Get buttons and containers
    let reportButton = document.getElementById("btnReportes");
    let reportContainer = document.getElementById("reportContainer");
    let autoReportButton = document.getElementById("btnPreciosAnomalos");
    let autoReportContainer = document.getElementById("automaticReportContainer");

    // Add event listener to the button
    reportButton.addEventListener("click", function () {
        reportContainer.classList.remove("d-none");
        autoReportContainer.classList.add("d-none");
    });

    autoReportButton.addEventListener("click", function () {
        autoReportContainer.classList.remove("d-none");
        reportContainer.classList.add("d-none");
    });
});