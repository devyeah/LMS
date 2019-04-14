# Software Requirements Specification for *LMS*

## 1.  Introduction

### 1.1  What’s *LMS*

LMS is a world-class online-learning management web application that makes it really easy for creators to build own online school.

### 1.2  Scope of *LMS*

LMS cannot produce any content about courses and it just helps instructors build own online course system.

## 2. Functional Requirements

### 2.1  Students Use Case

#### 2.1.1  Sign up

Students should be able to sign up LMS by entering their full name, email address, setting a password, and consenting to **LMS ’s Terms of Use and Privacy Policy** by checking the box.

![](img/LMS-Requirement-Student-Signup-01.png)


#### 2.1.2   Login

Students should be able to sign up LMS by entering an **email address** and **password** to access courses.

![](img/LMS-Requirement-Student-Login-01.png)


#### 2.1.3   Log out

Students should be able to sign up LMS by clicking the **Log Out** button.


#### 2.1.4  Edit Profile

(1) Students should be able to edit full name by re-entering full name.

(2) Students should be able to edit the email address by re-entering an email address.

(3) Students should be able to change the password by re-entering password twice.

(4) Once students have finished edit profile, they should save their changes by clicking **save changes** button.

![](img/LMS-Requirement-Student-EditProfile-01.png)


#### 2.1.5  Reset Password

(1)  If students forget own password, they should be able to send a request to reset their password by clicking **Forgot Password?** link, entering own email address into the **Email Address** field and then clicking **Send Me Instructions** button.

![](img/LMS-Requirement-Student-ResetPasswrod-01.png)

![](img/LMS-Requirement-Student-ResetPasswrod-02.png)



(2) Once students send a request, they should be able to reset the password by clicking **change my Password** button in the reset password email received in their email inbox.

![](img/LMS-Requirement-Student-ResetPasswrod-03.png)


#### 2.1.6  Navigate and view course content

(1) when students have logged in the website, they should be directed to the course view.

![](img/LMS-Requirement-Student-Content-01.png)

(2) Once students have clicked the course that they would like to access, students should be directed to the course’s curriculum view.

![](img/LMS-Requirement-Student-Content-02.png)


(3)  In course curriculum view students should be able to view the following information

- Course Progress

This progress bar displays the percentage of the course that you have completed.

![](img/LMS-Requirement-Student-Content-03.png)

- Class Curriculum

This displays all of the sections and lectures in the course.

![](img/LMS-Requirement-Student-Content-04.png)

- Your Instructor

This provides descriptions of the instructor teaching the course.

![](img/LMS-Requirement-Student-Content-05.png)

(4)  Students should be able to view each individual lecture when they click the **Start** button.

![](img/LMS-Requirement-Student-Content-06.png)

![](img/LMS-Requirement-Student-Content-07.png)

(5)  If students don’t enroll in this course, they should be redirected to the course’s sales page when they clicked the course that they would like to access.

![](img/LMS-Requirement-Student-Content-08.png)


(6) If students don’t enroll in this course, they should be able to see **Lecture content locked** instead of real content when they click the **Start** button next to individual lecture on the sales page.

![](img/LMS-Requirement-Student-Content-09.png)

(7) Students should be redirected to the next uncompleted lecture in the course when they click **Start next lecture** button at the top of the course curriculum view.

![](img/LMS-Requirement-Student-Content-10.gif)

(8) Students should be able to comment on individual lectures.

![](img/LMS-Requirement-Student-Content-11.gif)

(9) Students should be able to go back to edit, delete own comment built before.

![](img/LMS-Requirement-Student-Content-12.gif)

![](img/LMS-Requirement-Student-Content-13.gif)

(10) Students should be able to use online quiz and get a score when they completed all quizzes.

![](img/LMS-Requirement-Student-Content-14.gif)

