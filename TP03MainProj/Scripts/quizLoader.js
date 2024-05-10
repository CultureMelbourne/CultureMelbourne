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
        var cultureFromViewBag = $('#culture').data('culture').toLowerCase();
        // Assuming you want to standardize to lowercase
        $.getJSON('/Events/LoadQuizData', { culture: cultureFromViewBag }, function (data) {


            if (data) {
                startMessage.hide();
                loadQuizButton.hide();
                highScoreDisplay.show();

                startQuiz(data);

            } else {
                alert('No quiz data available for this culture.');
            }
        });
    });


    function startQuiz(quizData) {
        $('#quizQuestions').empty();  // Clear previous questions if any
        console.log(quizData);

        quizData.Questions.forEach(question => {
            let questionElem = $('<div>').addClass('question');
            let questionText = $('<h4>').text(question.Question);
            questionElem.append(questionText);

            let optionsList = $('<ul>');
            question.Options.forEach(option => {
                let optionItem = $('<li>');
                let optionButton = $('<button>').addClass('btn btn-secondary option-button')
                    .text(option)
                    .data('correct', option === question.CorrectAnswer);
                optionButton.on('click', function () {
                    if ($(this).data('correct')) {
                        alert('Correct!');
                    } else {
                        alert('Incorrect, try again.');
                    }
                });
                optionItem.append(optionButton);
                optionsList.append(optionItem);
            });

            questionElem.append(optionsList);
            $('#quizQuestions').append(questionElem);
        });
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