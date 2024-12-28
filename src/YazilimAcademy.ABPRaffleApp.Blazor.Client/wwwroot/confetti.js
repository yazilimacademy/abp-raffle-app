window.startConfetti = function () {
    var duration = 3 * 1000;
    var end = Date.now() + duration;

    // create a random style
    (function frame() {
        // launch confetti
        confetti({
            particleCount: 3,
            angle: 60,
            spread: 55,
            origin: { x: 0 }
        });
        confetti({
            particleCount: 3,
            angle: 120,
            spread: 55,
            origin: { x: 1 }
        });

        // keep going until time is up
        if (Date.now() < end) {
            requestAnimationFrame(frame);
        }
    }());
}
