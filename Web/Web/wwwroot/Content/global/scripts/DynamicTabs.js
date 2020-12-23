// 2018.3.23 rebuilt by YZ add rolling, closeall, closeother
// 2018.4.28 updated by YZ hide iframe => switch z-index 
// 2019.9.9  updated by YZ add shade before iframe loaded
window.onresize = setIframeHeight;

$(function ()
{
    setIframeHeight();

    $(".J_menuItem").on("click", c);
    $(".J_menuTabs").on("click", ".J_menuTab i", h);
    $(".J_tabCloseOther").on("click", i);
    $(".J_tabShowActive").on("click", j);
    $(".J_menuTabs").on("click", ".J_menuTab", e);
    $(".J_menuTabs").on("dblclick", ".J_menuTab", x);
    $(".J_tabLeft").on("click", a);
    $(".J_tabRight").on("click", b);
    $(".J_tabCloseAll").on("click", function ()
    {
        $(".page-tabs-content").children("[data-id]").not(":first").each(function ()
        {
            $('.J_iframe[data-id="' + $(this).data("id") + '"]').remove();
            $(this).remove()
        });
        $(".page-tabs-content").children("[data-id]:first").each(function ()
        {
            $('.J_iframe[data-id="' + $(this).data("id") + '"]').removeClass("ifr-below");
            $(this).addClass("active")
        });
        $(".page-tabs-content").css("margin-left", "0")
    })
});
var height = function () { return document.body.clientHeight - 115; }
function a()
{
    var o = Math.abs(parseInt($(".page-tabs-content").css("margin-left")));
    var l = f($(".content-tabs").children().not(".J_menuTabs"));
    var k = $(".content-tabs").outerWidth(true) - l;
    var p = 0;
    //if ($(".page-tabs-content").width() < k)
    if (!k)
    {
        return false
    }
    else
    {
        var m = $(".J_menuTab:first");
        var n = 0;
        while ((n + $(m).outerWidth(true)) <= o)
        {
            n += $(m).outerWidth(true);
            m = $(m).next()
        }
        n = 0;
        if (f($(m).prevAll()) > k)
        {
            while ((n + $(m).outerWidth(true)) < (k) && m.length > 0)
            {
                n += $(m).outerWidth(true);
                m = $(m).prev()
            }
            p = f($(m).prevAll())
        }
    }
    $(".page-tabs-content").animate({
        marginLeft: 0 - p + "px"
    },
        "fast")
}
function b()
{
    var o = Math.abs(parseInt($(".page-tabs-content").css("margin-left")));
    var l = f($(".content-tabs").children().not(".J_menuTabs"));
    var k = $(".content-tabs").outerWidth(true) - l;
    var p = 0;
    if ($(".page-tabs-content").width() < k)
    {
        return false
    } else
    {
        var m = $(".J_menuTab:first");
        var n = 0;
        while ((n + $(m).outerWidth(true)) <= o)
        {
            n += $(m).outerWidth(true);
            m = $(m).next()
        }
        n = 0;
        while ((n + $(m).outerWidth(true)) < (k) && m.length > 0)
        {
            n += $(m).outerWidth(true);
            m = $(m).next()
        }
        p = f($(m).prevAll());
        if (p > 0)
        {
            $(".page-tabs-content").animate({
                marginLeft: 0 - p + "px"
            },
                "fast")
        }
    }
}
function i()
{
    $(".page-tabs-content").children("[data-id]").not(":first").not(".active").each(function ()
    {
        $('.J_iframe[data-id="' + $(this).data("id") + '"]').remove();
        $(this).remove()
    });
    $(".page-tabs-content").css("margin-left", "0")
}
function j()
{
    g($(".J_menuTab.active"))
}
function e()
{
    if (!$(this).hasClass("active"))
    {
        var k = $(this).data("id");
        $(".J_mainContent .J_iframe").each(function ()
        {
            if ($(this).data("id") == k)
            {
                $(this).removeClass("ifr-below").siblings(".J_iframe").addClass("ifr-below");
                return false
            }
        });
        $(this).addClass("active").siblings(".J_menuTab").removeClass("active");
        g(this)
    }
}
function c()
{
    var o = $(this).attr("href"),
        m = $(this).data("index"),
        l = $.trim($(this).text()),
        k = true;
    if (o == undefined || $.trim(o).length == 0)
    {
        return false
    }
    $(".J_menuTab").each(function ()
    {
        if ($(this).data("id") == m)
        {
            if (!$(this).hasClass("active"))
            {
                $(this).addClass("active").siblings(".J_menuTab").removeClass("active");
                g(this);
                $(".J_mainContent .J_iframe").each(function ()
                {
                    if ($(this).data("id") == m)
                    {
                        $(this).removeClass("ifr-below").siblings(".J_iframe").addClass("ifr-below");
                        return false
                    }
                })
            }
            k = false;
            return false
        }
    });
    if (k)
    {
        index = layer.load(2, { shade: [0.01, '#fff'] });
        var p = '<a href="javascript:;" class="active J_menuTab" data-id="' + m + '">' + l + ' <i class="fa fa-times-circle"></i></a>';
        $(".J_menuTab").removeClass("active");
        var n = '<iframe class="J_iframe" name="iframe_' + m + '" width="100%" height="' + height() + '" src="' + o + '" frameborder="0" data-id="' + m + '" seamless onload="z()"></iframe>';
        $(".J_mainContent").find("iframe.J_iframe").addClass("ifr-below").parents(".J_mainContent").append(n);
        $(".J_menuTabs .page-tabs-content").append(p);
        g($(".J_menuTab.active"))
    }
    return false
}
var index
function z()
{
    layer.close(index);
}
function d()
{
    var l = $('.J_iframe[data-id="' + $(this).data("id") + '"]');
    var k = l.attr("src")
}
function x()
{
    $(this).find("i").click();
}
function h()
{
    var m = $(this).parents(".J_menuTab").data("id");
    var l = $(this).parents(".J_menuTab").width();
    if ($(this).parents(".J_menuTab").hasClass("active"))
    {
        if ($(this).parents(".J_menuTab").next(".J_menuTab").size())
        {
            // 如果不是最后一个
            var k = $(this).parents(".J_menuTab").next(".J_menuTab:eq(0)").data("id");
            $(this).parents(".J_menuTab").next(".J_menuTab:eq(0)").addClass("active");
            $(".J_mainContent .J_iframe").each(function ()
            {
                if ($(this).data("id") == k)
                {
                    $(this).removeClass("ifr-below").siblings(".J_iframe").addClass("ifr-below");
                    return false
                }
            });
            var n = parseInt($(".page-tabs-content").css("margin-left"));
            if (n < 0)
            {
                $(".page-tabs-content").animate({
                    marginLeft: (n + l) + "px"
                },
                    "fast")
            }
            $(this).parents(".J_menuTab").remove();
            $(".J_mainContent .J_iframe").each(function ()
            {
                if ($(this).data("id") == m)
                {
                    $(this).remove();
                    return false
                }
            })
        }
        if ($(this).parents(".J_menuTab").prev(".J_menuTab").size())
        {
            // 如果不是第一个
            var k = $(this).parents(".J_menuTab").prev(".J_menuTab:last").data("id");
            $(this).parents(".J_menuTab").prev(".J_menuTab:last").addClass("active");
            $(".J_mainContent .J_iframe").each(function ()
            {
                if ($(this).data("id") == k)
                {
                    $(this).removeClass("ifr-below").siblings(".J_iframe").addClass("ifr-below");
                    return false
                }
            });
            $(this).parents(".J_menuTab").remove();
            $(".J_mainContent .J_iframe").each(function ()
            {
                if ($(this).data("id") == m)
                {
                    $(this).remove();
                    return false
                }
            })
            // 20180411 关闭最后一个tab不会回退到前边的bug
            g($(".J_menuTab.active"));
        }
    } else
    {
        $(this).parents(".J_menuTab").remove();
        $(".J_mainContent .J_iframe").each(function ()
        {
            if ($(this).data("id") == m)
            {
                $(this).remove();
                return false
            }
        });
        g($(".J_menuTab.active"))
    }
    return false
}
function f(l)
{
    var k = 0;
    $(l).each(function ()
    {
        k += $(this).outerWidth(true)
    });
    return k
}
/** 定位到指定的tab上 */
function g(n)
{
    var o = f($(n).prevAll()),
        q = f($(n).nextAll());
    var l = f($(".content-tabs").children().not(".J_menuTabs"));
    var k = $(".content-tabs").outerWidth(true) - l;
    var p = 0;
    if ($(".page-tabs-content").outerWidth() < k)
    {
        p = 0
    } else
    {
        if (q <= (k - $(n).outerWidth(true) - $(n).next().outerWidth(true)))
        {
            if ((k - $(n).next().outerWidth(true)) > q)
            {
                p = o;
                var m = n;
                while ((p - $(m).outerWidth()) > ($(".page-tabs-content").outerWidth() - k))
                {
                    p -= $(m).prev().outerWidth();
                    m = $(m).prev()
                }
            }
        } else
        {
            if (o > (k - $(n).outerWidth(true) - $(n).prev().outerWidth(true)))
            {
                p = o - $(n).prev().outerWidth(true)
            }
        }
    }
    $(".page-tabs-content").animate({
        marginLeft: 0 - p + "px"
    },
        "fast")
}

function setIframeHeight()
{
    var ifr = $('iframe.J_iframe');
    ifr.each(function () { $(this).height(height) })
}
//打开标签页的api方法,参数e:{href=地址,id=菜单id,name=中文名称}
function addNewTabItem(e)
{
    var o = e.href, m = e.id, l = e.name, k = true;
    if (o == undefined || $.trim(o).length == 0)
    {
        return false
    }
    $(".J_menuTab").each(function ()
    {
        if ($(this).data("id") == m)
        {
            $(this).removeClass("active").addClass("active").siblings(".J_menuTab").removeClass("active");
            g(this);
            $(".J_mainContent .J_iframe").each(function ()
            {
                if ($(this).data("id") == m)
                {
                    $(this).removeClass("ifr-below").siblings(".J_iframe").addClass("ifr-below");
                    $(this).attr('src', o);
                    return false
                }
            })
            k = false;
            return false
        }
    });
    if (k)
    {
        index = layer.load(2, { shade: [0.01, '#fff'] });
        var p = '<a href="javascript:;" class="active J_menuTab" data-id="' + m + '">' + l + ' <i class="fa fa-times-circle"></i></a>';
        $(".J_menuTab").removeClass("active");
        var n = '<iframe class="J_iframe" name="iframe_' + m + '" width="100%" height="' + height() + '" src="' + o + '" frameborder="0" data-id="' + m + '" seamless onload="z()"></iframe>';
        $(".J_mainContent").find("iframe.J_iframe").addClass("ifr-below").parents(".J_mainContent").append(n);
        $(".J_menuTabs .page-tabs-content").append(p);
        g($(".J_menuTab.active"))
    }
    return false
}
//客户端通过CEF调用的js接口,func=方法名,ag=json字符串格式参数
function calljs(func, ag)    
{
    var ifr = $('iframe.J_iframe:not(.ifr-below)')[0];
    if (typeof (ifr.contentWindow.eval(func)) == 'function')
    {
        ifr.contentWindow.eval(func).call(this, ag);
    }
}

function onInOutCodeRead(code)
{
    $.post("/api/car/checkcode", { rawCode: code });
}