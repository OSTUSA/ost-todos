Feature: Logout
	As a user
	I can logout if I am logged in

Scenario: Viewing the logout link
	Given I am on page "User.Login"
	When I submit the form using
	| Email          | Password |
	| test1@test.com | password |
	Then element "a[href='/user/logout']" should be visible

Scenario: Logout link disabled when not logged in
	Given I am on page "User.Login"
	Then element "a[href='/user/logout']" should not exist

Scenario: Logging out to destroy the cookie
	Given I am logged in
	When I visit "/user/logout"
	Then there should be no cookie named ".ASPXAUTH"
