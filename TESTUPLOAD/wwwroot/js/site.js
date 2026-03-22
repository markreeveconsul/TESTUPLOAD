function updateClock() {
    const clockEl = document.getElementById("clock");
    const dateEl = document.getElementById("date");

    if (!clockEl || !dateEl) return; // exit if elements don't exist

    const now = new Date();

    let hours = now.getHours();
    let minutes = now.getMinutes();
    let ampm = hours >= 12 ? "PM" : "AM";

    hours = hours % 12;
    hours = hours ? hours : 12;
    minutes = minutes.toString().padStart(2, '0');

    clockEl.innerText = `${hours}:${minutes} ${ampm}`;

    const options = { weekday: 'long', month: 'long', day: 'numeric', year: 'numeric' };
    dateEl.innerText = now.toLocaleDateString(undefined, options);
}

// Only call if elements exist
setInterval(updateClock, 1000);
updateClock();

function toggleMenu() {
    const menu = document.getElementById("adminMenu");
    if (menu) menu.classList.toggle("open");
}
document.addEventListener("DOMContentLoaded", function () {

    const nameInput = document.querySelector('input[name="StudentName"]');
    const idInput = document.querySelector('input[name="StudentId"]');

    // When pressing ENTER in Name field → move to ID
    nameInput.addEventListener("keydown", function (e) {
        if (e.key === "Enter") {
            e.preventDefault(); // stop form submit
            idInput.focus();    // move cursor
        }
    });

    // Optional: prevent Enter from submitting in ID field too
    idInput.addEventListener("keydown", function (e) {
        if (e.key === "Enter") {
            e.preventDefault(); // stop accidental submit
        }
    });

});