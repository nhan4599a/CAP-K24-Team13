class AnimationLoader {
    constructor(animationContainer, animationJsonPath) {
        this.animationContainer = animationContainer;
        this.animationJsonPath = animationJsonPath;
        $(this.animationContainer).html('<div id="animation-loading"></div>')
    }

    showAnimation() {
        $(this.animationContainer).css('display', 'block');
        this.animation = bodymovin.loadAnimation({
            container: document.getElementById('animation-loading'),
            path: this.animationJsonPath,
            renderer: 'svg',
            loop: true,
            autoplay: true
        });
    }

    showAnimationWhenProcessing(task) {
        this.showAnimation();
        task();
        this.hideAnimation();
    }

    hideAnimation() {
        if (!this.animation)
            this.animation.destroy();
        $(this.animationContainer).css('display', 'none');
    }
}