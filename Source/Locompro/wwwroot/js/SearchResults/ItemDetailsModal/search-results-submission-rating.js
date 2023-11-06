class SearchResultsSubmissionRating {
    constructor(submission, ratingCell) {
        this.submission = submission;
        this.ratingCell = ratingCell;
        this.stars = [];
    }

    buildRating() {
        const ratingStars = document.createElement("div");
        ratingStars.classList.add("rating");

        let rating = this.submission.Rating;
        for (let starIndex = 0; starIndex < 5; starIndex++) {
            let starRating = Math.max(0, Math.min(1, rating));
            rating--;
            this.stars.push(new RatingStar(ratingStars, this.stars, (starRating >= 0.5), starIndex, this.submission));
        }

        this.ratingCell.appendChild(ratingStars);
    }
}

class RatingStar {
    constructor(RatingDiv, starList, originalState, starIndex, submission) {
        this.ratingDiv = RatingDiv;
        this.starList = starList;
        this.colored = originalState;
        this.starIndex = starIndex;
        this.submission = submission;

        this.element = document.createElement("div");
        this.element.classList.add("star-container");
        this.element.classList.add("star-icon");

        this.element.textContent = "★";

        this.element.style.position = 'relative';
        this.element.style.display = 'inline-block';
        this.element.style.fontSize = '24px';
        this.element.style.color = originalState ? 'gold' : 'grey';

        this.ratingDiv.style.display = 'inline-block';
        this.element.style.display = 'inline-block';

        // Bind the event listeners to this class instance
        this.element.addEventListener("mouseover", () => this.updateStarLook(true));
        this.element.addEventListener("mouseout", () => this.updateStarLook(false));
        this.element.addEventListener("click", () => this.setRating());

        this.ratingDiv.appendChild(this.element);
    }

    setRating() {
        this.starList[0].updateStarRatings(0, this.starIndex + 1);

        this.notifyNewRatingToServer();
    }

    updateStarLook(isHovered) {
        if (isHovered) {
            let color = "gold";

            for (let currentStar of this.starList) {
                currentStar.element.style.color = color;
                if (currentStar === this) {
                    color = "grey";
                }
            }
        } else {
            for (let currentStar of this.starList) {
                currentStar.restoreToOriginalState();
            }
        }
    }

    restoreToOriginalState() {
        this.element.style.color = this.colored ? 'gold' : 'grey';
    }

    updateStarRatings(nextStarToUpdate, ratingSet) {
        let starRating = 0;

        if (ratingSet >= 1) {
            starRating = 1;
        }

        ratingSet = ratingSet - starRating;
        this.starList[nextStarToUpdate].colored = starRating >= 0.5;
        this.element.style.color = this.colored ? 'gold' : 'grey';

        nextStarToUpdate++;

        if (nextStarToUpdate === this.starList.length) {
            return;
        }

        this.starList[nextStarToUpdate].updateStarRatings(nextStarToUpdate, ratingSet);
    }

    notifyNewRatingToServer() {
        let url = window.location.pathname;
        url += "?handler=UpdateSubmissionRating";

        this.submission.Rating = this.starIndex + 1;

        let dataToSend = {
            submissionUserId: this.submission.UserId,
            submissionEntryTime: this.submission.NonFormatedEntryTime,
            Rating: "" + this.submission.Rating
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
}