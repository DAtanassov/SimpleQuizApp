# MVC Exam - 25.10.2025 (Simple Quiz App)

*(Quiz Application - allows users to answer multiple-choice questions and see their score after submission.)*

## Instructions


### To start the application:
	
1. through Visual Studio (with/without debug)
	
2. to publish on a web server, for example IIS:
	
	- must have "runtime aspnetcore 9.0.10" and "runtime aspnetcor hosting bundle 9.0.10" installed from the [Microsoft website](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
	
	- published in a directory
	
	- and a new site is added to IIS

### Usage

When opening in a web browser the address where the application is published
		or launched through Visual Studio, the home page with Quizzes opens

![Web Gui Preview](https://github.com/DAtanassov/SimpleQuizApp/HomePage.png)

The page shows the available tests to solve,
	a field for a file from which to import more Quizzes and a link
	with a sample file for import.

To start the Quiz, the user must mark the radio button in front of it
	and press the "Start Quiz" button. You will be redirected to another page
	with the questions and possible answers (each question has one possible answer).

![Web Gui Preview](https://github.com/DAtanassov/SimpleQuizApp/Quiz.png)

On the page you can see the name of the Quiz, below it a timer that counts down
		the time for automatic completion of the Quiz depending on the number of questions
		The user has two minutes for each question to solve it.
						
On the left a progress bar that decreases according to the remaining time.

On the right a progress bar that increases depending on the answers given.
 
Below the questions there is a counter showing the number of marked out of the total number of questions.

Below it are several buttons:
- "Back to Home" - redirect to the home page for selecting a Quiz

- "Reload Quiz" - reloads the current Quiz, but with shuffled questions and answers
	
- "Reset answers" - clears the marked answers to the questions
	
- "Submit Quiz" - sends the user's selection for processing and redirects to the page for visualizing the results

![Web Gui Preview](https://github.com/DAtanassov/SimpleQuizApp/Results.png)

The page displays:
	
- "Total Questions" - the total number of questions from the Quiz
	
- "Answers" - the total number of questions the user has answered *(if time runs out, he may not have answered all of them)*
	
- "Correct Answers" - the total number of correct answers
	
- "Score" - the points received depending on the answers / maximum points. *(minimum for a successful Quiz is 75 points, with all questions having equal weight)*

Below them a message about a successful or unsuccessful Quiz.

Below it a button "Take Another Quiz" - a link to the home page for selecting a Quiz

## Note!

**The application does not work with a database!**

When launched, sample questions will be loaded from a [file](https://github.com/DAtanassov/SimpleQuizApp/blob/master/JSONfiles/InitialQuestions.json), which will be copied when publishing the application.

In the same [directory](https://github.com/DAtanassov/SimpleQuizApp/tree/master/JSONfiles) you can also find a sample [file](https://github.com/DAtanassov/SimpleQuizApp/blob/master/JSONfiles/QuizzesForImport.json) with several Quizzes for import
	