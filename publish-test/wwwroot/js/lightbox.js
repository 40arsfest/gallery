export function registerSwipe(element, dotNetRef) {
    let startX = 0;
    let startY = 0;

    const onTouchStart = (e) => {
        startX = e.touches[0].clientX;
        startY = e.touches[0].clientY;
    };

    const onTouchEnd = (e) => {
        const endX = e.changedTouches[0].clientX;
        const endY = e.changedTouches[0].clientY;
        const diffX = endX - startX;
        const diffY = endY - startY;

        if (Math.abs(diffX) < 50 || Math.abs(diffY) > Math.abs(diffX))
            return;

        if (diffX > 0)
            dotNetRef.invokeMethodAsync('OnSwipeRight');
        else
            dotNetRef.invokeMethodAsync('OnSwipeLeft');
    };

    element.addEventListener('touchstart', onTouchStart, { passive: true });
    element.addEventListener('touchend', onTouchEnd, { passive: true });

    return {
        dispose: () => {
            element.removeEventListener('touchstart', onTouchStart);
            element.removeEventListener('touchend', onTouchEnd);
        }
    };
}
