class AnimationLoader {
    constructor(animationContainer, animationJsonPath) {
        this.animationContainer = animationContainer;
        this.animationJsonPath = animationJsonPath;
        this.isAnimationShowing = false;
        $(this.animationContainer).html('<div id="animation-loading"></div>')
    }

    showAnimation(timeInMillis) {
        if (this.isAnimationShowing)
            return;
        $(this.animationContainer).css('display', 'block');
        this.animation = bodymovin.loadAnimation({
            container: document.getElementById('animation-loading'),
            path: this.animationJsonPath,
            renderer: 'svg',
            loop: true,
            autoplay: true
        });
        this.isAnimationShowing = true;
        if (timeInMillis > 0)
            setTimeout(_this => {
                _this.hideAnimation();
            }, timeInMillis, this);
    }

    hideAnimation() {
        if (!this.isAnimationShowing)
            return;
        if (!this.animation)
            this.animation.destroy();
        $(this.animationContainer).css('display', 'none');
    }
}