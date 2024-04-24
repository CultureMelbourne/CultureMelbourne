document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('loadQuiz').addEventListener('click', function () {
        var button = this;
        var message = document.getElementById('startMessage');
        var quizFrameDiv = document.getElementById('quizFrame');

        button.style.display = 'none';  // For hiding the button
        message.style.display = 'none'; // For hiding the messgaes

        var cultureName = document.getElementById('culture').getAttribute('data-culture');
        var iframeSrc = '';

        // Below are temporary embed links (Real ones from Tally.so after Siqi modifies)
        switch (cultureName) {
            case 'Japanese':
                iframeSrc = 'https://tally.so/embed/wxyz123?hideTitle=1&align=center';
                break;
            case 'Chinese':
                iframeSrc = 'https://tally.so/embed/wvNDOg?alignLeft=1&hideTitle=1&transparentBackground=1&dynamicHeight=1';
                break;
            case 'Korean':
                iframeSrc = 'https://tally.so/embed/3EPlbl?alignLeft=1&hideTitle=1&transparentBackground=1&dynamicHeight=1';
                break;
            case 'Filipino':
                iframeSrc = 'https://tally.so/embed/ijkl101?hideTitle=1&align=center';
                break;
            case 'Vietnamese':
                iframeSrc = 'https://tally.so/embed/mRzoYK?alignLeft=1&hideTitle=1&transparentBackground=1&dynamicHeight=1';
                break;
            default:
                iframeSrc = '';
                break;
        }

        var iframe = document.createElement('iframe');
        iframe.src = iframeSrc;
        iframe.width = '100%';
        iframe.height = '500';
        iframe.frameBorder = '0';
        iframe.marginHeight = '0';
        iframe.marginWidth = '0';
        iframe.title = 'Cultural Quiz';

        quizFrameDiv.appendChild(iframe);
        quizFrameDiv.style.display = 'block';
    });
});
