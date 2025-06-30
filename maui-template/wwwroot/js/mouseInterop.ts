// 定义 .NET 对象类型
type DotNetObject = {
    invokeMethodAsync: (method: string, ...args: any[]) => void;
};

declare global {
    interface Window {
        mouseInterop: {
            subscribeMouseMove: (dotNetObjRef: DotNetObject) => void;
            unsubscribeMouseMove: () => void;
        };
        mouseDragInterop: {
            startDrag: (dotNetObjRef: DotNetObject) => void;
        };
    }
}

window.mouseInterop = {
    subscribeMouseMove: function (dotNetObjRef: DotNetObject) {
        window.onmousemove = function (e: MouseEvent) {
            dotNetObjRef.invokeMethodAsync('OnGlobalMouseMove', e.screenX, e.screenY);
        };
    },
    unsubscribeMouseMove: function () {
        window.onmousemove = null;
    }
};

window.mouseDragInterop = {
    startDrag: function (dotNetObjRef: DotNetObject) {
        function onMouseMove(e: MouseEvent) {
            dotNetObjRef.invokeMethodAsync('OnGlobalMouseMove', e.screenX, e.screenY);
        }
        function onMouseUp(e: MouseEvent) {
            dotNetObjRef.invokeMethodAsync('OnGlobalMouseUp');
            window.removeEventListener('mousemove', onMouseMove);
            window.removeEventListener('mouseup', onMouseUp);
        }
        window.addEventListener('mousemove', onMouseMove);
        window.addEventListener('mouseup', onMouseUp);
    }
};