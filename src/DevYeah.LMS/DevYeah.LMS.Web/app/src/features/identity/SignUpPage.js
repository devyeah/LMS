import React, { Component } from 'react';
import SignUpForm from './SignUpForm';
import './identity.css';

export default class SignUpPage extends Component {
  constructor(props){
    super(props);

    this.submitHandler = this.submitHandler.bind(this);
  }

  submitHandler(values, actions){
    setTimeout(() => {
      console.log(JSON.stringify(values));
      actions.setSubmitting(false);
      actions.resetForm();
    }, 1000);
  }

  render(){
    return (
      <div className="signup-page-color">
        <SignUpForm submitHandler={this.submitHandler} />
      </div>
    );
  }
}