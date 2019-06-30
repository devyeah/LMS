import React, { Component } from 'react';
import { connect } from 'react-redux';
import { formMeta, formValidation} from './formMeta/signUpFormMeta';
import { signup } from './redux/actions';
import DynamicForm from './DynamicForm';

class SignUp extends Component {
  constructor(props){
    super(props);

    this.submitHandler = this.submitHandler.bind(this);
  }

  submitHandler(values, actions){
    this.props.onSubmit(values)
      .then(
        (error) => {
          actions.setSubmitting(false);
          if (!error) {
            actions.resetForm();
          }
        }
      );
  }

  render(){
    const { error } = this.props;

    return (
      <div className="h-100">
        <DynamicForm 
          formName="signUpForm"
          submitHandler={this.submitHandler} 
          formValidation={formValidation}
          formMeta={formMeta}
          error={error ? error : undefined}
        />
      </div>
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
  {onSubmit: signup}
)(SignUp);