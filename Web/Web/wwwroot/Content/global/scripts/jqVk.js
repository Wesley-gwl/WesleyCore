/**
 * 要配合css和固定的html 功能单一
 * 最适合只有一个文本框需要使用虚拟键盘的情况
 * 如果有多个文本框需要输入 使用比方式比较复杂
 */
var jqVk = (function () {
    var jqVk = function (options) {
        return new jqVk.fn.init(options);
    }

    jqVk.fn = jqVk.prototype = {
        constructor: jqVk,
        writer: null,
        onDeletehandler: [],
        onInputhandler: [],
        bgImg: null,
        init: function (options) {
            var jsVk = this;
            var $write = null,
                shift = false,
                capslock = false;

            $('#keyboard li').click(function () {

                if (jsVk.writer != null)
                    $write = $("#" + jsVk.writer);

                var $this = $(this),
                    character = $this.html(); // If it's a lowercase letter, nothing happens to this variable

                // Shift keys
                if ($this.hasClass('left-shift') || $this.hasClass('right-shift')) {
                    $('.letter').toggleClass('uppercase');
                    $('.symbol span').toggle();

                    shift = (shift === true) ? false : true;
                    capslock = false;
                    return false;
                }

                // Caps lock
                if ($this.hasClass('capslock')) {
                    $('.letter').toggleClass('uppercase');
                    capslock = true;
                    return false;
                }
                $("#" + jsVk.writer).focus();
                var doDef = true;
                // Delete
                if ($this.hasClass('delete')) {

                    //执行配置的事件
                    for (var i = 0; i < jsVk.onDeletehandler.length; i++) {
                        if (!jsVk.onDeletehandler[i](jsVk.writer)) {
                            doDef = false
                        }
                    }
                    if (doDef && $write) {
                        jsVk.Delete()
                    }
                    return false;
                }


                // Special characters
                if ($this.hasClass('symbol')) character = $('span:visible', $this).html();
                if ($this.hasClass('space')) character = ' ';
                if ($this.hasClass('tab')) character = "\t";
                if ($this.hasClass('return')) character = "\n";

                // Uppercase letter
                if ($this.hasClass('uppercase')) character = character.toUpperCase();

                // Remove shift once a key is clicked.
                if (shift === true) {
                    $('.symbol span').toggle();
                    if (capslock === false) $('.letter').toggleClass('uppercase');

                    shift = false;
                }

                //执行配置的事件
                for (var i = 0; i < jsVk.onInputhandler.length; i++) {
                    if (!jsVk.onInputhandler[i](jsVk.writer, character)) {
                        doDef = false
                    }
                }
                if (!doDef || !$write) {
                    return;
                }

                // Add the character
                jsVk.setValue(character);
            });
        },
        Delete: function () {
            if (!this.writer)
                return;
            var write = $("#" + this.writer);

            var html = write.html();
            if (write[0].tagName == "INPUT") {
                html = write.val();
                write.val(html.substr(0, html.length - 1));
            } else {
                write.html(html.substr(0, html.length - 1));
            }

        },
        setValue: function (v) {
            if (!this.writer)
                return;
            var write = $("#" + this.writer);

            var html = write.html();
            if (write[0].tagName == "INPUT") {
                html = write.val();
                write.val(html + v);
            } else {
                write.html(html + v);
            }
        },
        on: function (eName, func) {
            switch (eName) {
                case "Delete":
                    if (func && typeof (func) === "function") {
                        this.onDeletehandler.push(func);
                    }
                    break;
                case "Input":
                    if (func && typeof (func) === "function") {
                        this.onInputhandler.push(func);
                    }
                    break;
                default:
                    break;

            }
            return this;
        }
    }
    jqVk.fn.init.prototype = jqVk.fn;
    return jqVk;
})();