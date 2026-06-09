export function canShare() {
    return typeof navigator.share === 'function';
}

export async function sharePhoto({ url, title, text, fileName }) {
    if (!navigator.share)
        return { ok: false, reason: 'unsupported' };

    const shareTitle = title || '40 år – Fotoarkivet';

    try {
        const response = await fetch(url, { mode: 'cors' });
        if (response.ok) {
            const blob = await response.blob();
            const ext = blob.type === 'image/png' ? '.png' : '.jpg';
            const file = new File([blob], fileName || `foto${ext}`, {
                type: blob.type || 'image/jpeg'
            });

            if (navigator.canShare?.({ files: [file] })) {
                await navigator.share({ files: [file], title: shareTitle, text: text || '' });
                return { ok: true };
            }
        }
    } catch {
        // Fall back to sharing the image URL.
    }

    try {
        await navigator.share({
            title: shareTitle,
            text: text || '',
            url
        });
        return { ok: true };
    } catch (e) {
        if (e?.name === 'AbortError')
            return { ok: true };

        return { ok: false, reason: e?.message || 'failed' };
    }
}

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
