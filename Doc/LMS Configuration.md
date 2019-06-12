# Configuration Required for Running LMS

## 1. Server Side Config

Some essential settings needs to be set before starting server side software.

### 1.1 Email Settings

Some important functions of LMS needs to send email to users. In order for these functions to work properly, system needs some relevant information.

Open appsettings.json


### 1.2 Token Settings

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