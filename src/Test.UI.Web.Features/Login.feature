Feature: Login
	As a user
	I can login to my Pomodoro Power account

Scenario: Logging in with a valid user
	Given I am on page "User.Login"
	When I submit the form using
	| Email          | Password |
	| test1@test.com | password |
	Then A cookie named ".ASPXAUTH" should exist
	And I should be redirected to "/"

Scenario: Logging in and omitting the email address
	Given I am on page "User.Login"
	When I submit the form using
	| Email | Password |
	|       | password |
	Then element "span[data-valmsg-for='Email']" should have text

Scenario: Logging in and omitting the password
	Given I am on page "User.Login"
	When I submit the form using
	| Email               | Password |
	| scaturrob@gmail.com |          |
	Then element "span[data-valmsg-for='Password']" should have text

Scenario: Logging in with an invalid email
	Given I am on page "User.Login"
	When I submit the form using
	| Email          | Password |
	| test@gmail.com | password |
	Then element "span[data-valmsg-for='Email']" should have text

Scenario: Logging in with an invalid password
	Given I am on page "User.Login"
	When I submit the form using
	| Email          | Password    |
	| test1@test.com | badpassword |
	Then element "span[data-valmsg-for='Email']" should have text
