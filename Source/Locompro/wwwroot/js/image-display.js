let curretSlideIndex = 1;

// Next/previous controls
function plusSlides(slideIndex) {
    showSlides(curretSlideIndex += slideIndex);
}

function showSlides(slideIndex) {
    let slides = document.getElementsByClassName("mySlides");
    
    if (slideIndex > slides.length) {
        curretSlideIndex = 1;
    }
    if (slideIndex < 1) {
        curretSlideIndex = slides.length;
    }
    for (let currentSlide = 0; currentSlide < slides.length; currentSlide++) {
        slides[currentSlide].style.display = "none";
    }

    slides[curretSlideIndex-1].style.display = "block";
}

