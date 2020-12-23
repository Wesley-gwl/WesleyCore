var signAture = (function (window) {
    var signAture = function (selector) {
        return new signAture.fn.init(selector);
    }

    signAture.fn = signAture.prototype = {
        constructor: signAture,
        init: function (selector) {
            this.mousePressed = false;
            this.lastX = 0
            this.lastY = 0;
            this.c = document.querySelector('#' + selector);
            this.ctx = this.c.getContext("2d");
            this.InitThis();
        },
        getImageBase64: function () {
            var image = this.c.toDataURL("image/png");

            return image;
        },

        InitThis: function () {
            var sig = this;
            //触摸屏
            sig.c.addEventListener('touchstart', function (event) {
                console.log(1)
                if (event.targetTouches.length == 1) {
                    event.preventDefault();// 阻止浏览器默认事件，重要
                    var touch = event.targetTouches[0];
                    sig.mousePressed = true;
                    var rect = this.getBoundingClientRect();
                    sig.Draw(touch.pageX - rect.top, touch.pageY - rect.left, false);
                }

            }, false);

            sig.c.addEventListener('touchmove', function (event) {
                console.log(2)
                if (event.targetTouches.length == 1) {
                    event.preventDefault();// 阻止浏览器默认事件，重要
                    var touch = event.targetTouches[0];
                    if (sig.mousePressed) {
                        var rect = this.getBoundingClientRect();
                        sig.Draw(touch.pageX - rect.left, touch.pageY - rect.top, true);
                    }
                }

            }, false);

            sig.c.addEventListener('touchend', function (event) {
                console.log(3)
                if (event.targetTouches.length == 1) {
                    event.preventDefault();// 阻止浏览器默认事件，防止手写的时候拖动屏幕，重要
                    sig.mousePressed = false;
                }
            }, false);

            //		   鼠标
            sig.c.onmousedown = function (event) {
                sig.mousePressed = true;
                var rect = this.getBoundingClientRect();
                sig.Draw(event.pageX - rect.left, event.pageY - rect.top, false);
            };

            sig.c.onmousemove = function (event) {
                if (sig.mousePressed) {
                    var rect = this.getBoundingClientRect();

                    sig.Draw(event.pageX - rect.left, event.pageY - rect.top, true);

                }
            };

            this.c.onmouseup = function (event) {
                sig.mousePressed = false;
            };
        },

        Draw: function (x, y, isDown) {
            if (isDown) {
                this.ctx.beginPath();
                this.ctx.strokeStyle = 'black';
                this.ctx.lineWidth = 3;
                this.ctx.lineJoin = "round";
                this.ctx.moveTo(this.lastX, this.lastY);
                this.ctx.lineTo(x, y);
                this.ctx.closePath();
                this.ctx.stroke();
            }
            this.lastX = x; this.lastY = y;
        },

        clearArea: function () {

            this.ctx.setTransform(1, 0, 0, 1, 0, 0);
            this.ctx.clearRect(0, 0, this.ctx.canvas.width, this.ctx.canvas.height);
        }
    }

    signAture.fn.init.prototype = signAture.fn;

    return signAture;
})();