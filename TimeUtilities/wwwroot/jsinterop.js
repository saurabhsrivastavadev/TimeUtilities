/* File containing functions for JS Interop */

//-------------------------------------------------------------
// Timezone info utilities
//-------------------------------------------------------------
window.getLocalTimezoneOffset = () => {
    return new Date().getTimezoneOffset();
}

window.getLocalTimezoneName = () => {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
}
//-------------------------------------------------------------


//-------------------------------------------------------------
// App triggered PWA install prompt
// reference : https://web.dev/customize-install/
//-------------------------------------------------------------
let pwaInstallPrompt;
let pwaIsInstalled = true;

window.addEventListener('beforeinstallprompt', (e) => {

    console.log('beforeinstallprompt event');

    pwaIsInstalled = false;

    // Prevent the mini-infobar from appearing on mobile
    e.preventDefault();
    // Stash the event so it can be triggered later.
    pwaInstallPrompt = e;

    console.log("saved the pwa install prompt for app trigger.");
});

window.showPwaInstallPrompt = () => {

    if (pwaInstallPrompt) {

        pwaInstallPrompt.prompt();

        pwaInstallPrompt.userChoice.then((choiceResult) => {

            if (choiceResult.outcome === 'accepted') {
                console.log('user accepted install prompt.');
                return true;
            } else {
                console.log('user rejected install prompt.');
                return false;
            }
        });
    }
}

window.addEventListener('appinstalled', (e) => {

    console.log('appinstalled event');
    pwaIsInstalled = true;
});

window.isPwaInstalled = () => {

    return pwaIsInstalled;
}

//-------------------------------------------------------------
