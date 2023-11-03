let currentSlideIndex = 1;

// Next/previous controls
function plusSlides(slideIndex) {
    showSlides(currentSlideIndex += slideIndex);
}

function showSlides(slideIndex) {
    let slides = document.getElementsByClassName("mySlides");
    
    if (slideIndex > slides.length) {
        currentSlideIndex = 1;
    }
    if (slideIndex < 1) {
        currentSlideIndex = slides.length;
    }
    for (let currentSlide = 0; currentSlide < slides.length; currentSlide++) {
        slides[currentSlide].style.display = "none";
    }

    slides[currentSlideIndex-1].style.display = "block";
}

