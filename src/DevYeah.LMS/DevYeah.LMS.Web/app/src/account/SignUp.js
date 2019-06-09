import React, { Component } from 'react';
import SignUpForm from './SignUpForm';
import './account.css';

export default class SignUp extends Component {
  constructor(props){
    super(props);

    this.submitHandler = this.submitHandler.bind(this);
  }

  submitHandler(values, actions){
    //todo: submit values to backend server
    actions.setSubmitting(false);
    actions.resetForm();
  }

  render(){
    return (
      <div className="bg-color">
        <SignUpForm submitHandler={this.submitHandler} />
      </div>
    );
  }
}