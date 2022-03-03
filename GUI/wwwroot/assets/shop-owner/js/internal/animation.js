class AnimationLoader {
    constructor(animationContainer, animationJsonPath) {
        this.animationContainer = animationContainer;
        this.animationJsonPath = animationJsonPath;
        this.isAnimationShowing = false;
        this.timeoutId = null;
        this.animationCompletedCallback = null;
        this.animationLoop = true;
    }

    showAnimation(timeInMillis) {
        if (this.isAnimationShowing)
            return;
        $(this.animationContainer).parent().css('display', 'block');
        this.animation = bodymovin.loadAnimation({
            container: document.querySelector(this.animationContainer),
            path: this.animationJsonPath,
            renderer: 'svg',
            loop: this.animationLoop,
            autoplay: true
        });
        this.isAnimationShowing = true;
        if (this.animationCompletedCallback)
            this.animation.addEventListener('complete', this.animationCompletedCallback);
        if (timeInMillis > 0)
            this.timeoutId = setTimeout(_this => {
                _this.hideAnimation();
                _this.timeoutId = null;
            }, timeInMillis, this);
    }

    hideAnimation(shouldInvokingCallback = false) {
        if (!this.isAnimationShowing)
            return;
        if (this.animation) {
            this.animation.stop();
            this.animation.destroy();
        }
        if (this.animationCompletedCallback && shouldInvokingCallback)
            this.animationCompletedCallback();
        if (this.timeoutId)
            clearTimeout(this.timeoutId);
        $(this.animationContainer).parent().css('display', 'none');
    }

    setAnimationCompletedCallback(callback) {
        if (typeof callback != 'function')
            throw new Error('callback must be a function');
        this.animationCompletedCallback = callback;
    }

    setAnimationLoop(count) {
        if (typeof count == 'number') {
            if (count >= 0) {
                this.animationLoop = count;
                return;
            }
        }
        if (typeof count == 'boolean') {
            this.animationLoop = count;
            return;
        }
        throw new Error('Invalid animation loop');
    }
}