var fabricHelper = (function (window) {
    var fabricHelper = fabric.util.createClass(/** @lends fabric.Object.prototype */ {
        id: "",
        fontSize: 18,
        rackColor: null,
        doorColor: null,
        rackImgSrc: null,
        doorImgSrc: null,
        rackImgDom: null,
        doorImgDom: null,
        rackRedImg: null,
        textBgColor: null,
        beforObjRender: null,
        bgColor: null,
        bgImg: null,
        initialize: function (canvasId, options) {
            if (!canvasId || typeof (canvasId) !== "string")
                throw "canvasBoxId mast be string ";

            options || (options = {});
            var help = this;
            help.id = canvasId;
            help.ratioW = 1;//仓库x轴和画布像素宽的比例
            help.ratioH = 1;//仓库y轴和画布像素高的比例

            help.canvas = new fabric.Canvas(help.id);
            var canvasBox = $("#" + help.id).parent().parent();
            //根据配置进行操作 修改object类的原型属性值
            var opt = getOpts(options);
            $.extend(help, opt);

            help.transparentCorners(help.isTransparentCorners);
            help.lockMovement(help.isLockMovement);
            help.lockScaling(help.isLockScaling);
            help.setSelection(help.isSelection);
            fabric.Group.prototype.hasControls = help.isHasControls;

            //设置背景 图或颜色
            if (opt.bgImg && opt.bgImg.length > 4) {
                this.bgImg = opt.bgImg;
                help.setBackgroundColor(opt.bgImg);
            } else if (opt.bgColor) {
                this.bgColor = opt.bgColor;
                help.canvas.setBackgroundColor(opt.bgColor, help.canvas.renderAll.bind(help.canvas));
            }

            //单独加载图片 加载完毕后遍历画布上所有对象 设置 他们的背景
            fabric.Image.fromURL(help.rackImg, function (img) {
                help.rackImgDom = img.getElement();
                help.setAllObjBgImg(help, 0, help.rackImgDom);
            });
            //单独加载图片 加载完毕后遍历画布上所有对象 设置 他们的背景
            fabric.Image.fromURL(help.rackRedImg, function (img) {
                rackRedImgElement = img.getElement();
            });

            //单独加载图片 加载完毕后遍历画布上所有对象 设置 他们的背景
            fabric.Image.fromURL(this.doorImg, function (img) {
                help.doorImgDom = img.getElement();
                help.setAllObjBgImg(help, 1, help.doorImgDom)
            });

            if (typeof (opt.beforObjRender) == "function") {
                this.beforObjRender = opt.beforObjRender;
            }

            //双击画布上的对象
            if (opt.dbClick && typeof (opt.dbClick) === "function") {
                canvasBox.dblclick(function () {
                    var obj = help.canvas.getActiveObject();
                    if (!obj) return;
                    opt.dbClick(obj);
                });
            }
            //双击画布
            if (opt.canvasDbClick && typeof (opt.canvasDbClick) === "function") {
                canvasBox.dblclick(function () {
                    var obj = help.canvas.getActiveObject();
                    if (obj) return;
                    opt.canvasDbClick();
                });
            }

            //支持单击
            if (opt.click && typeof (opt.click) === "function") {
                canvasBox.click(function () {
                    var obj = help.canvas.getActiveObject();
                    if (!obj) return;
                    opt.click(obj);
                });
            }
            //只处理需要内部处理的任务
            help.canvas.on({
                'object:modified': function (e) {
                    //console.log("modified")
                    var obj = e.target;
                    if (obj.type !== 'group') return;
                    //修正描边宽度
                    var rect = obj.item(0)
                    rect.strokeWidth = 3;
                    //重写设置背景
                    //var bgImg = rect.Type == 0 ? help.rackImgDom : help.doorImgDom;
                    //help.setObjBgImg(rect, bgImg);

                    //禁用文字缩放  删除重新加
                    var text = obj.getObjects("text")[0];
                    var ag = 0;
                    if ((text.text.length * help.fontSize) > obj.getWidth())
                        ag = 90;
                    var newText = new fabric.Text(text.text, getTextOpts({
                        left: obj.left,
                        top: obj.top,
                        fontSize: help.fontSize,
                        textBackgroundColor: help.textBgColor, angle: ag
                    }));
                    obj.remove(text);
                    obj.addWithUpdate(newText);
                    console.log(this);
                },
                'object:moving': function (e) { checkPosition(e.target) },
            });
            //画布大小跟随父容器大小
            help.resizeCanvas(canvasBox.width(), canvasBox.height());
        },
        setBackgroundColor: function (url) {
            this.canvas.setBackgroundColor({
                //source: 'http://fabricjs.com/assets/escheresque_ste.png',
                source: url,
                repeat: 'repeat',
                //offsetX: 200,//偏移位置
                //offsetY: 100
            }, this.canvas.renderAll.bind(this.canvas));
        },
        lockScaling: function (b) {
            //禁止缩放
            fabric.Group.prototype.lockScalingY = b;
            fabric.Group.prototype.lockScalingX = b;
        },
        transparentCorners: function (b) {
            //选中框 显示为 透明
            fabric.Object.prototype.transparentCorners = b;
        },
        lockMovement: function (b) {
            //禁止拖动
            fabric.Group.prototype.lockMovementY = b;
            fabric.Group.prototype.lockMovementX = b;
            if (b) {
                //设置鼠标滑过时鼠标的样式 defaultCursor 即无样式
                this.canvas.on('mouse:over', function (event) {
                    if (event.target !== null)
                        event.target.hoverCursor = event.target.canvas.defaultCursor;
                });
            }
        },
        setBgImg: function (imgUrl) {
            //设置背景
            //加载背景图 //'/Content/images/rackdesign/timg.jpg'
            var fbHelp = this;
            fabric.Image.fromURL(imgUrl, function (img) {
                if (!img) return;

                img.scaleToWidth(fbHelp.canvas.getWidth());
                img.scaleToHeight(fbHelp.canvas.getHeight());
                fbHelp.canvas.setBackgroundImage(img);
                fbHelp.canvas.renderAll();
            });
        },
        AddGroup: function (shapeInfo) {
            //向画布上添加一个矩形和文本的组
            return this.AddGroupToCanvas(this.canvas, shapeInfo);
        },
        AddGroupToCanvas: function (canvas, shapeInfo) {
            //向指定的画布上添加一个矩形和文本的组
            if (!shapeInfo.shLength)
                shapeInfo.shLength = 0;

            if (!shapeInfo.shWidth)
                shapeInfo.shWidth = 0;

            var wth = this.Mul(shapeInfo.shLength, this.ratioW);
            var ht = this.Mul(shapeInfo.shWidth, this.ratioH);
            var item1 = new fabric.Rect({
                code: shapeInfo.code,
                id: shapeInfo.id,
                storageId: shapeInfo.storageId,
                shHeight: shapeInfo.shHeight,
                shWidth: shapeInfo.shWidth,
                shLength: shapeInfo.shLength,
                //srLayers: shapeInfo.srLayers,
                type: shapeInfo.type,
                name: shapeInfo.name,
                width: wth,
                height: ht,
                stroke: '#fff',
                strokeWidth: 2,
                fill: "",//shapeInfo.Type == 0 ? this.rackColor : this.doorColor,
                originX: 'center', //调整中心点的X轴坐标  即该组图形的对齐方式
                originY: 'center',
            });
            var bgImg = shapeInfo.type == 0 ? this.rackImgDom : this.doorImgDom;
            if (bgImg)
                this.setObjBgImg(item1, bgImg);
            //textbox 初始化时候的文字如果超过 width 则会自动换行 否则不会
            //text 和它的子类不能 y轴缩放
            var ag = 0;
            if (shapeInfo.name.length * this.fontSize > wth)
                ag = 90;
            var item3 = new fabric.Text(shapeInfo.name, getTextOpts({
                fontSize: this.fontSize,
                textBackgroundColor: this.textBgColor, angle: ag
            }));

            //进行组合
            //位置判断 避免显示不全
            var minX = this.division(shapeInfo.shLength, 2);
            var minY = this.division(shapeInfo.shWidth, 2);
            if (!shapeInfo.srx)
                shapeInfo.srx = 0;

            if (!shapeInfo.sry)
                shapeInfo.sry = 0;
            var srx = (shapeInfo.srx > minX ? shapeInfo.srx : minX);
            var sry = (shapeInfo.sry > minY ? shapeInfo.sry : minY);
            //item2,
            var group = new fabric.Group([item1, item3], {
                //lockRotation: true,
                hasRotatingPoint: false,
                hasBorder: true,
                //borderColor: "#000",
                originY: 'center',
                originX: 'center',
                width: wth,
                height: ht,
                shHeight: shapeInfo.shHeight,
                shWidth: shapeInfo.shWidth,
                shLength: shapeInfo.shLength,
                left: this.Mul(srx, this.ratioW),
                top: this.Mul(sry, this.ratioH)
            });

            if (this.beforObjRender)
                this.beforObjRender(group);
            this.canvas.add(group);
            return group;
        },
        UpdateObj: function (data) {
            //更新画布上的对象 删除重新添加
            var obj = this.canvas.getActiveObject();
            if (!obj) return;
            this.canvas.remove(obj);

            this.AddGroup(data);
        },
        ConvertCanvasToObj: function (canvas) {
            //将画布上的图形组转换为数据对象
            var groups = this.canvas.getObjects("group");
            if (!groups || groups.length < 1)
                return [];
            var objList = [];
            for (var i = 0; i < groups.length; i++) {
                var item = this.ConvertGroupToObj(groups[i]);
                if (!item) continue;
                objList.push(item);
            }
            return objList;
        },
        ConvertGroupToObj: function (group) {
            //将组转换成货架
            var item = {};
            var locationInfo = group.item(0);
            if (!locationInfo)
                return false;

            if (locationInfo) {
                item = {
                    id: locationInfo.id,
                    code: locationInfo.code,
                    shHeight: locationInfo.shHeight,
                    //srLayers: locationInfo.srLayers,
                    shLength: this.division(group.getWidth(), this.ratioW),
                    shWidth: this.division(group.getHeight(), this.ratioH),
                    srx: this.division(group.left, this.ratioW),
                    sry: this.division(group.top, this.ratioH),
                    storageId: locationInfo.storageId,
                    name: locationInfo.name,
                    type: locationInfo.type,
                }
                if (locationInfo.hasOwnProperty("locationGuid"))
                    item.locationGuid = locationInfo.locationGuid;
            }
            return item;
        },
        GetCanvasData: function () {
            //获取数据
            var data = this.ConvertCanvasToObj(this.canvas);
            return data;
        },
        resizeCanvasAll: function (w, h) {
            //设置宽度随容器改变 画布重新渲染
            var datas = this.GetCanvasData();
            var canvasBox = $("#" + this.id).parent().parent();

            //设置宽度充满容器
            if (!h) h = canvasBox.height();
            if (!w) w = canvasBox.width();
            this.canvas.setWidth(w);
            this.canvas.setHeight(h);
            this.resetCanvas();
            if (datas && datas.length > 0)
                this.AddRacks(datas, this.storeWidth, this.storeHeight);

            this.canvas.renderAll();
        },
        resizeCanvas: function (w, h) {
            //获取画布上所有元素 统一放大或缩小
            //简单的方法 画布重新渲染
            this.canvas.setHeight(h);
            this.canvas.setWidth(w);
            this.canvas.renderAll();
        },
        resizeCanvasAllByData: function (data, storeWidth, storeHeight) {
            //设置宽度随容器改变   简单的方法 画布重新渲染
            this.resetCanvas();
            var canvasBox = $("#" + this.id).parent().parent();
            //设置宽度充满容器
            this.canvas.setHeight(canvasBox.height());
            this.canvas.setWidth(canvasBox.width());

            this.AddRacks(data, storeWidth, storeHeight);

            this.canvas.renderAll();
        },
        AddRacks: function (data, storeWidth, storeHeight) {
            if (!storeWidth || !storeHeight)
                throw "storeWidth and storeHeight mast has value";
            this.storeHeight = storeHeight;
            this.storeWidth = storeWidth;
            this.ratioW = this.division(this.canvas.getWidth(), storeWidth);
            this.ratioH = this.division(this.canvas.getHeight(), storeHeight);
            for (var i = 0; i < data.length; i++) {
                //根据仓库和货架的大小比例来计算 货架在画布上的宽高
                this.AddGroup(data[i]);
            }
        },
        resetCanvas: function () {
            //重置画布
            this.canvas.clear();
            //设置背景 图或颜色
            if (this.bgImg) {
                this.setBackgroundColor(this.bgImg);
            } else if (this.bgColor) {
                this.canvas.setBackgroundColor(this.bgColor, this.canvas.renderAll.bind(this.canvas));
            }
        },
        setSelection: function (b) {
            this.canvas.selection = b;
        }, setAllObjBgImg(help, type, img) {
            var groups = help.canvas.getObjects("group");
            if (!groups || groups.length < 1)
                return;

            for (var i = 0; i < groups.length; i++) {
                var item = groups[i].getObjects("rect")[0];
                if (item.Type != type) continue;
                this.setObjBgImg(item, img)
            }
            help.canvas.renderAll();
        },
        setObjBgImg: function (obj, img) {
            if (!obj) return;
            var newImg = $(img).clone();
            newImg.id = "";
            newImg.width(obj.width);
            newImg.height(obj.height);

            obj.setPatternFill({
                source: newImg[0],
                repeat: "no-repeat",
            })
        },
        setObjImgFlicker: function (obj) {
            if (!obj) return;
            var newImg = $(rackRedImgElement).clone();
            newImg.id = "";
            newImg.width(obj.width);
            newImg.height(obj.height);
            obj.setPatternFill({
                source: newImg[0],
                repeat: "no-repeat",
                offsetX: 0,
                offsetY: 0,
            })
        },
        onObjClcik: function (func, isUnbindElse) {
            //支持单击
            var help = this;
            if (typeof (func) !== "function") return;
            var canvasBox = $("#" + help.id).parent().parent();
            //取消其他点击事件
            if (isUnbindElse) canvasBox.unbind("click");
            canvasBox.click(function () {
                var obj = help.canvas.getActiveObject();
                if (!obj) return;
                func(obj);
            });
        },
        division: function (arg1, arg2) {
            //规避小数计算问题
            var t1 = 0, t2 = 0, r1, r2;
            try { t1 = arg1.toString().split(".")[1].length } catch (e) { }
            try { t2 = arg2.toString().split(".")[1].length } catch (e) { }
            with (Math) {
                r1 = Number(arg1.toString().replace(".", ""))
                r2 = Number(arg2.toString().replace(".", ""))
                return (r1 / r2) * pow(10, t2 - t1);
            }
        },
        Mul: function (arg1, arg2) {
            //规避小数计算问题
            var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
            try { m += s1.split(".")[1].length } catch (e) { }
            try { m += s2.split(".")[1].length } catch (e) { }
            return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m)
        }
    });
    //对象位置检查  防止移出画布边界
    function checkPosition(obj) {
        // if object is too big ignore
        if (obj.currentHeight > obj.canvas.height || obj.currentWidth > obj.canvas.width) {
            return false;
        }
        obj.setCoords();
        // top-left  corner
        if (obj.getBoundingRect().top < 0 || obj.getBoundingRect().left < 0) {
            obj.top = Math.max(obj.top, obj.top - obj.getBoundingRect().top);
            obj.left = Math.max(obj.left, obj.left - obj.getBoundingRect().left);
        }
        // bot-right corner
        if (obj.getBoundingRect().top + obj.getBoundingRect().height > obj.canvas.height || obj.getBoundingRect().left + obj.getBoundingRect().width > obj.canvas.width) {
            obj.top = Math.min(obj.top, obj.canvas.height - obj.getBoundingRect().height + obj.top - obj.getBoundingRect().top);
            obj.left = Math.min(obj.left, obj.canvas.width - obj.getBoundingRect().width + obj.left - obj.getBoundingRect().left);
        }
    };

    function getOpts(opts) {
        var defaltOpt = {
            isSelection: true,//鼠标框选
            isHasControls: true,//选中时显示外框
            isTransparentCorners: false,//选中时的外框透明
            isLockMovement: false,//禁止移动
            isLockScaling: false,//禁止缩放
            dbClick: null,//画布上的对象的双击
            canvasDbClick: null,//画布的双击（鼠标位置无图像时）
            click: null,//画布上图像的单击
            bgColor: null,
            bgImg: null,
            fontSize: 18,
            beforObjRender: null,
            doorColor: "#84a",
            rackColor: "#84aee6",
            textBgColor: "#fff",
            rackImg: "/content/image/storageDesign/rack.svg",
            rackRedImg: "/content/image/storageDesign/rackRed.svg",
            doorImg: "/content/image/storageDesign/door.svg"
        };
        if (!opts) return defaltOpt;

        for (i in defaltOpt) {
            if (opts.hasOwnProperty(i)) {
                defaltOpt[i] = opts[i];
            }
        }
        return defaltOpt;
    };

    function getTextOpts(opts) {
        var defaltOpt = {
            angle: 0,
            fontFamily: "Microsoft YaHei,arial,sans-serif,tahoma",
            fontSize: 18,
            lineHeigth: 36,
            height: 36,
            textAlign: 'center',
            ry: 3,
            rx: 3,
            textBackgroundColor: "",
            originY: 'center',
            originX: 'center',
            left: null,
            top: null,
        }

        if (!opts) return defaltOpt;

        for (i in defaltOpt) {
            if (opts.hasOwnProperty(i)) {
                defaltOpt[i] = opts[i];
            }
        }
        return defaltOpt;
    }

    return fabricHelper;
})(window);