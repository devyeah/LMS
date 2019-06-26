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
    //todo: submit values to backend server
    this.props.onSubmit(values);
    if (!this.props.error) {
      actions.setSubmitting(false);
      actions.resetForm();
    }
  }

  render(){
    const { errorMessage } = this.props;

    return (
      <div className="h-100">
        <DynamicForm 
          formName="signUpForm"
          submitHandler={this.submitHandler} 
          formValidation={formValidation}
          formMeta={formMeta}
        />
        {errorMessage 
          && <div className="alert alert-danger" role="alert">{errorMessage}</div>}
        
      </div>
    );
  }
}

const mapStateToProps = (state) => {
  return {
    error: state.authenticate.errorMessage
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    onSubmit: signup
  };
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(SignUp);