
import Vue from 'https://cdn.jsdelivr.net/npm/vue@2.6.14/dist/vue.esm.browser.js'

Vue.directive('mask', VueTheMask.mask);

new Vue({
    el: "#app",
    data() {
        return {
            currentCardBackground: Math.floor(Math.random() * 25 + 1), // just for fun :D
            cardName: "",
            cardNumber: "",
            cardMonth: "",
            cardYear: "",
            cardCvv: "",
            minCardYear: new Date().getFullYear(),
            otherCardMask: "#### #### #### ####",
            cardNumberTemp: "",
            isCardFlipped: false,
            focusElementStyle: null,
            isInputFocused: false
        };
    },
    mounted() {
        this.cardNumberTemp = this.otherCardMask;
        document.getElementById("cardNumber").focus();
    },
    computed: {
        getCardType() {
            return "visa"; // default type
        },
        generateCardNumberMask() {
            return this.otherCardMask;
        },
        minCardMonth() {
            if (this.cardYear === this.minCardYear) return new Date().getMonth() + 1;
            return 1;
        }
    },
    watch: {
        cardYear() {
            if (this.cardMonth < this.minCardMonth) {
                this.cardMonth = "";
            }
        }
    },
    methods: {
        flipCard(status) {
            this.isCardFlipped = status;
        },
        focusInput(e) {
            this.isInputFocused = true;
            let targetRef = e.target.dataset.ref;
            let target = this.$refs[targetRef];
            this.focusElementStyle = {
                width: `${target.offsetWidth}px`,
                height: `${target.offsetHeight}px`,
                transform: `translateX(${target.offsetLeft}px) translateY(${target.offsetTop}px)`
            }
        },
        blurInput() {
            let vm = this;
            setTimeout(() => {
                if (!vm.isInputFocused) {
                    vm.focusElementStyle = null;
                }
            }, 300);
            vm.isInputFocused = false;
        }
    }
});

$('.card-form__button').click(function () {
    let animationLoader = new AnimationLoader('#loading-container > #animation-container', '/assets/user/checking-out.json');
    animationLoader.showAnimation();
    setTimeout(() => {
        animationLoader.hideAnimation();
        window.location = '/order-history';
    }, 2000);
});