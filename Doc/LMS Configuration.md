# Configuration Required for Running LMS

## 1. Server Side Config

Some essential settings needs to be set before starting server side software.

### 1.1 Email Settings

Some important functions of LMS needs to send email to users. In order for these functions to work properly, system needs some relevant information. Follow the instruction below to set them:

1. Open appsettings.json
2. Find out "EmailSettings" section 
3. Set properties
   - **OfficialEmailAddress:**  the email of your website; 
   - **Port:** the port of your email server;
   - **UseSsl:** whether to use SSL when sending email;
   - **AccountName:** the name used to login your email server;
   - **Password:** password used to login your email server;
   - **MaxRetryCount:** the times that you intend to resend email when the email failed to send at first time;


### 1.2 Token Settings

LMS uses JWT token to implement some security related features, such as login. The essential properties is following:

1. Open appsettings.json
2. Find out "TokenSettings" section 
3. Set properties
   - **Issuer:** issuer;
   - **Audience:** audience;
   - **Secret:** will be used to build SymmetricSecurityKey;
   - **Expires:** token will expire after this;

## 2 App Config

LMS is built on many open-source software that distributed via npm which is bundled with Node. Please make sure [Node](https://nodejs.org/) has been installed in the environment where you intend to run LMS on.

### 2.1 Installation

To run properly, make sure all the dependencies for the LMS are installed:

npm:

`cd (your app folder)`

`npm install`

yarn:

`cd (your app folder)`

`yarn`