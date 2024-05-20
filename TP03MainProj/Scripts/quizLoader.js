$(document).ready(function () {
    $(document).on('change', '.btn-check', optionClicked);
    $(document).on('submit', 'form', handleSubmit);
    const startMessage = $('#startMessage');
    const loadQuizButton = $('#loadQuiz');
    const highScoreDisplay = $('#highScoreDisplay');
    const highScoreValue = $('#highScoreValue');
    var highScore = 0;  // Session high score
    var currentScore = 0;
    const quizQuestions = $('#quizQuestions');
    const quizContent = $('#quizContent');
    const restartQuizButton = $('#restartQuiz').hide();
    const prevQuestionButton = $('#prevQuestion');
    const nextQuestionButton = $('#nextQuestion');
    const progressbar = $('#progress_area');
    const cheerSound = $('#cheerSound')[0];
    const cheerAnimation = $('#cheerAnimation');
    var quizData;

    loadQuizButton.on('click', function () {
        var culture = $('#culture').data('culture').toLowerCase();
        loadQuiz(culture);
    });

    function loadQuiz(culture) {
        $.getJSON('/Events/LoadQuizData', { culture: culture }, function (data) {
            if (data) {
                quizData = data;
                startMessage.hide();
                loadQuizButton.hide();
                highScoreDisplay.show();
                progressbar.show();
                initializeQuiz(quizData);
            } else {
                alert('No quiz data available for this culture.');
            }
        });
    }

    function initializeQuiz(quizData) {
        quizContent.empty(); // 只清空问题内容，不清空整个quizQuestions
        currentQuestionIndex = 0; // Reset to start from the first question
        var cultureFromViewBag = quizData.Name.toLowerCase();

        quizData.Questions.forEach((question, index) => {
            const questionElem = createQuestionElement(question, index, cultureFromViewBag);
            quizContent.append(questionElem);
        });

        setupNavigation(quizData.Questions.length);
        showQuestion(currentQuestionIndex); // Show the first question

        let lastQuestionForm = $('#question' + (quizData.Questions.length - 1) + ' form');
        lastQuestionForm.append($('<button>', {
            type: 'submit',
            class: 'btn btn-success mt-3 submit-button',
            text: 'Complete Quiz'
        }));

        updateProgressBar(0, quizData.Questions.length); // Initialize progress bar
    }

    function createQuestionElement(question, index, cultureFromViewBag) {
        let questionElem = $('<div>', {
            class: 'question card shadow-lg p-5 bg-body rounded-5 col-12 mx-auto border rounded',
            id: 'question' + index
        }).hide();

        let questionHeader = $('<div>', {
            class: 'fs-2 mb-3 fw-bold text-center'
        }).text(`Question ${index + 1}: ${question.Question}`);

        let imagePath = `/Content/Images/${cultureFromViewBag}/${question.QuestionNum}`;
        let image = $('<img>', {
            src: `${imagePath}.jpg`,
            class: 'img-fluid rounded-start p-3 quiz-img',
            alt: 'Quiz Image',
            onerror: `this.src='${imagePath}.png'`
        });

        let form = $('<form>', {
            class: 'card-body d-flex flex-column align-items-center justify-content-center'
        });

        let optionsContainer = $('<div>', {
            class: 'container'
        });

        question.Options.forEach(option => {
            let optionId = `question${index}-option${option}`;

            let optionRow = $('<div>', {
                class: 'row justify-content-center'
            });

            let optionCol = $('<div>', {
                class: 'col-12 col-md-8 justify-content-center'
            });

            let optionTextSpan = $('<span>', {
                class: 'small-text',
                style: 'display: block;'
            }).text(option);

            let optionLabel = $('<label>', {
                class: 'list-group-item list-group-item-action text-center border border-dark rounded-3 shadow p-2 mb-3 fs-6',
                for: optionId,
                style: 'white-space: normal; overflow-wrap: break-word;'
            });

            let optionInput = $('<input>', {
                type: 'radio',
                class: 'btn-check',
                id: optionId,
                name: `question${index}-options`,
                'data-correct': option === question.CorrectAnswer
            }).on('change', optionClicked);

            optionLabel.append(optionInput, optionTextSpan);
            optionCol.append(optionLabel);
            optionRow.append(optionCol);
            optionsContainer.append(optionRow);
        });

        form.append(optionsContainer);

        let cardRow = $('<div>', { class: 'row g-0' });
        let imageCol = $('<div>', { class: 'col-md-4' }).append(image);
        let textCol = $('<div>', { class: 'col-md-8' }).append(form);

        cardRow.append(imageCol, textCol);
        questionElem.append(questionHeader, cardRow);
        return questionElem;
    }

    function optionClicked() {
        let isCorrect = $(this).data('correct');
        let questionElem = $(this).closest('.question');
        let correctAnswerLabel = questionElem.find('input[data-correct="true"]').parent('label');
        let correctAnswer = correctAnswerLabel.text();

        // Mark only if not previously attempted
        if (!questionElem.data('attempted')) {
            questionElem.data('attempted', true);
            if (isCorrect) {
                currentScore++;
                $(this).closest('label').addClass('list-group-item-success').removeClass('list-group-item-action');
            } else {
                $(this).closest('label').addClass('list-group-item-danger').removeClass('list-group-item-action');

                // 使用 Bootstrap 模态框显示消息
                const messageHtml = `<span class="text-danger fw-bold fs-4">Incorrect!</span>  The answer is:  <span class="fw-bold fs-4">${correctAnswer}</span> `;
                $('#messageModalBody').html(messageHtml);
                var messageModal = new bootstrap.Modal(document.getElementById('messageModal'));
                messageModal.show();
            }
        }
    }


    function handleSubmit(event) {
        event.preventDefault(); // Prevent the default form submission
        endQuiz(); // Call to handle the end of the quiz
    }

    function endQuiz() {
        showFinalMessage(currentScore, highScore);
        $('#quizContent').hide();
        $('#restartQuiz').show();
        $('#prevQuestion').hide();
        $('#nextQuestion').hide();
        progressbar.hide();

        updateHighScore(currentScore); // Update the high score if necessary
    }

    function resetQuiz() {
        currentScore = 0;
        $('#quizContent').empty().show();
        startMessage.show();
        loadQuizButton.show();
        restartQuizButton.hide();
        highScoreDisplay.hide();
        startMessage.hide();
        loadQuizButton.hide();
        progressbar.show();

        $('#scoreDisplay').hide(); // Hide the score display when restarting
        initializeQuiz(quizData); // Re-initialize the quiz
    }

    $(document).on('click', '#restartQuiz', function () {
        resetQuiz(); // Reset the quiz when the restart button is clicked
    });

    function setupNavigation(totalQuestions) {
        prevQuestionButton.click(() => navigate(-1, totalQuestions));
        nextQuestionButton.click(() => navigate(1, totalQuestions));
        updateButtonVisibility(currentQuestionIndex, totalQuestions);
    }

    function navigate(direction, totalQuestions) {
        let newIndex = currentQuestionIndex + direction;
        if (newIndex >= 0 && newIndex < totalQuestions) {
            currentQuestionIndex = newIndex;
            showQuestion(currentQuestionIndex);
            updateButtonVisibility(currentQuestionIndex, totalQuestions);
            updateProgressBar(currentQuestionIndex, totalQuestions); // Update progress bar
        }
    }

    function showQuestion(index) {
        $('.question').hide().eq(index).fadeIn(400);
    }

    function updateButtonVisibility(currentIndex, totalQuestions) {
        $('#prevQuestion').toggle(currentIndex > 0);
        $('#nextQuestion').toggle(currentIndex < totalQuestions - 1);
    }

    function updateProgressBar(currentIndex, totalQuestions) {
        const progressBar = $('#quizProgress');
        const progress = ((currentIndex + 1) / totalQuestions) * 100;
        progressBar.css('width', progress + '%').attr('aria-valuenow', progress).text(`Question ${currentIndex + 1} of ${totalQuestions}`);
    }

    function showFinalMessage(score, highScore) {
        // Update and show the score display
        $('#finalScore').text(score); // Display the final score
        $('#scoreDisplay').show().hide().fadeIn(1000); // Show the score display section with animation

        // Animate the final score value
        $({ countNum: 0 }).animate({ countNum: score }, {
            duration: 2000,
            easing: 'swing',
            step: function () {
                $('#finalScore').text(Math.floor(this.countNum));
            },
            complete: function () {
                $('#finalScore').text(this.countNum);
            }
        });

        // If the score is a high score, play cheer sound and animation
        if (score >= highScore) {
            var cheerSound = document.getElementById('cheerSound');
            cheerSound.volume = 1.0; // Ensure volume is at max
            cheerSound.play();

            // 播放喝彩动画
            $('#cheerAnimation').fadeIn(500, function () {
                setTimeout(function () {
                    var fadeOutInterval = setInterval(function () {
                        if (cheerSound.volume > 0.05) {
                            cheerSound.volume -= 0.05;
                            $('#cheerAnimation').css('opacity', cheerSound.volume); // 同步调整动画透明度
                        } else {
                            cheerSound.volume = 0;
                            cheerSound.pause();
                            clearInterval(fadeOutInterval);
                            $('#cheerAnimation').fadeOut(500); // 动画完全淡出
                        }
                    }, 100); // 每100ms降低一次音量和透明度
                }, 2000); // 2秒后开始淡出
            });
        }

        let message;
        if (score > highScore) {
            message = 'Great job! New high score!';
        } else if (score > 0) {
            message = 'Nice try! Maybe review some more and try again?';
        } else {
            message = 'Seems like you could learn some more about this culture!';
        }

        // 使用 Bootstrap 模态框显示消息
        $('#messageModalBody').text(message);
        var messageModal = new bootstrap.Modal(document.getElementById('messageModal'));
        messageModal.show();

        // Show the high score in the display if needed
        $('#highScoreValue').text(highScore);
        $('#highScoreDisplay').show().hide().fadeIn(1000); // Show the high score display with animation
        $('#restartQuizButton').show().hide().fadeIn(1000); // Ensure the restart button is shown with animation
    }


    function updateHighScore(currentScore) {
        if (currentScore > highScore) {
            highScore = currentScore;
            $('#highScoreValue').text(highScore);
        }
    }

});
