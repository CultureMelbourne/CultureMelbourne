// new custom implementation

$(document).ready(function () {
    const quizSection = $('#quiz-section');
    const startMessage = $('#startMessage');
    const loadQuizButton = $('#loadQuiz');
    const quizFrame = $('#quizFrame');
    const highScoreDisplay = $('#highScoreDisplay');
    const highScoreValue = $('#highScoreValue');
    var highScore = 0;  // Session high score
    var currentScore = 0;

    loadQuizButton.click(function () {
        // Redundant method Ensuring culture name is in lowercase
        //const culture = $('#culture').data('culture').toLowerCase();
        $.getJSON('/Events/LoadQuizData', { culture: cultureFromViewBag }, function (data) {
            const questionsData = data.cultures.find(c => c.name.toLowerCase() === cultureFromViewBag);
            if (questionsData) {
                startMessage.hide();
                loadQuizButton.hide();
                highScoreDisplay.show();
                startQuiz(questionsData.questions);
            } else {
                alert('No quiz data available for this culture.');
            }
        });
    });


    function startQuiz(questions) {
        quizFrame.html(''); // Clear previous quiz content
        currentScore = 0; // Reset current score
        questions.forEach(question => {
            let questionElem = $('<div>').addClass('question').text(question.question);
            let imagePath = `/Content/Images/${cultureFromViewBag}/${question.questionNum}`;
            let image = $('<img>').attr('src', `${imagePath}.jpg`).addClass('quiz-image').on('error', function () {
                $(this).attr('src', `${imagePath}.png`); // For JPG or PNG check
            });

            quizFrame.append(questionElem, image);
            let optionsContainer = $('<div>').addClass('options-container');
            question.options.forEach(option => {
                let optionButton = $('<button>').addClass('option').text(option).click(function () {
                    handleAnswer(option, question.correctAnswer, $(this));
                });
                optionsContainer.append(optionButton);
            });
            quizFrame.append(optionsContainer);
        });
        quizFrame.append($('<button>').attr('id', 'restartQuiz').text('Restart Quiz').click(resetQuiz));
    }

    function handleAnswer(selectedOption, correctAnswer, optionButton) {
        $('.option').prop('disabled', true);  // Disable all options after a selection
        if (selectedOption === correctAnswer) {
            currentScore++;
            optionButton.addClass('correct-answer');
            alert('Correct!');
        } else {
            optionButton.addClass('wrong-answer');
            $('.option').each(function () {
                if ($(this).text() === correctAnswer) {
                    $(this).addClass('correct-answer');
                }
            });
            alert('Wrong answer!');
        }
        updateHighScore(currentScore);
        showFinalMessage(currentScore, highScore);  // Call to show the final message
    }

    function updateHighScore(newScore) {
        if (newScore > highScore) {
            highScore = newScore;
            highScoreValue.text(highScore);  // Update high score display
        }
    }

    function resetQuiz() {
        startMessage.show();
        loadQuizButton.show();
        quizFrame.html('');
        highScoreDisplay.hide();
    }

    function showFinalMessage(score, highScore) {
        let message = '';
        if (score >= highScore) {
            message = 'Great job! You know a lot about this culture.';
        } else if (score > 0) {
            message = 'Nice try! Maybe review some more and try again?';
        } else {
            message = 'Seems like you could learn some more about this culture!';
        }
        alert(message);
    }
});







/* Code for Tally.so implementation

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
                iframeSrc = 'https://tally.so/embed/wdY0Dr?alignLeft=1&hideTitle=1&transparentBackground=1&dynamicHeight=1';
                break;
            case 'Chinese':
                iframeSrc = 'https://tally.so/embed/wvNDOg?alignLeft=1&hideTitle=1&transparentBackground=1&dynamicHeight=1';
                break;
            case 'Korean':
                iframeSrc = 'https://tally.so/embed/3EPlbl?alignLeft=1&hideTitle=1&transparentBackground=1&dynamicHeight=1';
                break;
            case 'Filipino':
                iframeSrc = 'https://tally.so/embed/nWARpk?alignLeft=1&hideTitle=1&transparentBackground=1&dynamicHeight=1';
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
        iframe.height = '100%';
        iframe.frameBorder = '0';
        iframe.marginHeight = '0';
        iframe.marginWidth = '0';
        iframe.title = 'Cultural Quiz';
        

        quizFrameDiv.appendChild(iframe);
        quizFrameDiv.style.display = 'block';
    });
});
*/