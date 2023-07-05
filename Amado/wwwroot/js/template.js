let toTop = document.getElementById("toTop");
toTop.addEventListener("click", () => {
    document.body.scrollTop = 0; 
    document.documentElement.scrollTop = 0;
})