/* File containing functions for JS Interop */

// Get timezone offset for current locale
window.getLocalTimezoneOffset = () => {
    return new Date().getTimezoneOffset();
}

window.getLocalTimezoneName = () => {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
}
