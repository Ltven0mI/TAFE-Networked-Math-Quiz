# ClassroomPlanner Contribution Guide

When writing code for this project the following programming standards and
conventions should be followed at all times.
If required, an examption to these standards may be granted by your supervisor.

## Class / Variable Naming Conventions

### Instance / Member Variables

Use ['camelCase'](https://www.chaseadams.io/posts/most-common-programming-case-types/#camelcase)
which "``must (1) start with a lowercase letter and (2) the first letter of every new subsequent word has its first letter capitalized and is compounded with the previous word.``"
```csharp
int minFriends = 0;
int maxFriends = 2;
```

### Properties

Use ['PascalCase'](https://www.chaseadams.io/posts/most-common-programming-case-types/#pascalcase)
which "``has every word starts with an uppercase letter (unlike camelCase in that the first word starts with a lowercase letter).``"
```csharp
int MinFriends { get; set; } = 0;
int MaxFriends { get; set; } = 2;
```

### Constant / Static Variables

Use ['UPPER_CASE_SNAKE_CASE'](https://www.chaseadams.io/posts/most-common-programming-case-types/#upper_case_snake_case)
which "``is replacing all the spaces with a "_" and converting all the letters to capitals.``"
```csharp
const int MIN_FRIENDS = 0;
static int MAX_FRIENDS = 2;
```

### Methods

Use ['PascalCase'](https://www.chaseadams.io/posts/most-common-programming-case-types/#pascalcase)
which "``has every word starts with an uppercase letter (unlike camelCase in that the first word starts with a lowercase letter).``"
```csharp
int GetMinFriends() {};
int GetMaxFriends() {};
```

### Class Names

Use ['PascalCase'](https://www.chaseadams.io/posts/most-common-programming-case-types/#pascalcase)
which "``has every word starts with an uppercase letter (unlike camelCase in that the first word starts with a lowercase letter).``"
```csharp
class Friend {}
class FriendList : List<Friend> {}
```

## Comments

### File / Class Header Comments

All class files must have a commented file header which contains the name of
the file, purpose, author's name, version control number, date, and
testing notes (if any).
```csharp
/**********************************************************/
// Filename:   <class-file-name>
// Purpose:    <short-description>
// Author:     <your-name>
// Version:    <version-number>
// Date:       <date-of-current-version>
// Tests:      <unit-tests>
/**********************************************************/
```

E.g.
```csharp
/**********************************************************/
   // Filename: Friend.cs
   // Purpose: To represent a friend and their data.
   // Author: Wade Rauschenbach
   // Version: 0.3.0
   // Date: 21-Aug-2020
   // Tests: Unit test completed 25-Aug-2020
/**********************************************************/
```

### File / Class Changelog Comments

All class files must have a commented file changelog which is placed after
the file header comment with a single empty line between them.

The changelog must also follow the format / guidelines from [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)
```csharp
/*********************** Changelog ************************/
// [Unreleased]
// | [Changed]
// | - <upcoming-changes>
// 
// [<release-version>] <release-date>
// | [Added]
// | - <additions>
/**********************************************************/
```

```csharp
E.g.
   /*********************** Changelog ************************/
   // [Unreleased]
   //
   // [0.3.0] 23-Aug-2020
   // | [Added]
   // | - Add IsValidName(string name) for validating a name.
   // | - Add IsValidLikes(string likes) for validating likes.
   // | - Add IsValidDislikes(string dislikes) for validating dislikes.
   // | - Add IsValidBirthMonth(int month) for validating a birth month.
   // | - Add IsValidBirthDay(int day, int month) for validating a birth day.
   // | - Add CompareByValues(Friend other) for comparing friends by value.
   //
   // [0.2.0] 21-Aug-2020
   // | [Added]
   // | - Implement Clone() method for cloning an instance by value.
   //
   // [0.1.0] 07-Aug-2020
   // | [Added]
   // | - Initial Friend implementation.
   /**********************************************************/
```

### Method Header Comments

All methods must have a commented method header which contains the
method signature, purpose, inputs, and outputs.
```csharp
/**********************************************************/
// Method:  <method-signature>
// Purpose: <purpose-of-method>
// Returns: <1st-return-condition>
// Returns: <2nd-return-condition>
// Inputs:  <argument-type-and-names>
// Outputs: <return-type>
// Throws:  <potential-thrown-exception>
/**********************************************************/
```

E.g.
```csharp
/**********************************************************/
// Method:  public bool IsValidName (String name)
// Purpose: Validates whether the passed 'name' is a
//          valid name for a friend.
// Returns: true if 'name' is valid.
// Returns: false if 'name' is NOT valid.
// Inputs:  String name
// Outputs: bool
/**********************************************************/
```

### Source Code Comments

Comments in the source code should be avoided when possible.
Try to maintain readable and un-ambiguous code at all times.

Only include source code comments when the outcome of a block of code is not
instantly comprehensible.

### Tabs / Spacing

Appropriate tab indentations and spacing are to be used throughout to aid
readability and file management.

## Try / Catch Blocks

Appropriate try{} and catch{} blocks are to be used for the coding of database
connections, file i/o operations, error logging, or attempting any numeric
conversions from string values.

## User Input Sanitizing

All user inputs must be sanitized and validated to ensure accuracy and avoid
exceptions / security vulnerabilities.


