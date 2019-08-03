import React, { Component } from 'react';
import { connect } from 'react-redux';
import {formMeta, formValidation} from './formMeta/signInFormMeta';
import { signin } from './redux/actions';
import DynamicForm from './DynamicForm';

class SignIn extends Component {
  constructor(props){
    super(props);

    this.submitHandler = this.submitHandler.bind(this);
  }

  submitHandler(values, actions) {
    this.props.signin(values)
      .then(
        error => {
          actions.setSubmitting(false);
          if (!error) {
            actions.resetForm();
          }
        }
      );
  }

  render() {
    var { error } = this.props;
    return (
      <DynamicForm 
        formName="signInForm"
        formMeta={formMeta}
        formValidation={formValidation}
        submitHandler={this.submitHandler} 
        error={error ? error : undefined}
      />
    );
  }
}

const mapStateToProps = (state) => {
  return {
    error: state.authenticate.errorMessage
  };
};

export default connect(
  mapStateToProps,
  {signin}
)(SignIn);