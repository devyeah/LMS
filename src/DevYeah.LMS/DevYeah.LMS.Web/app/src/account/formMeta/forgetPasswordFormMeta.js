import * as yup from 'yup';

const formValidation = yup.object().shape({
  email: yup.string()
    .email('Invalid email address')
    .required('Required!'),
});

const formMeta = {
  initialValues: { email: '' },  
  header: {
    id:'resetPwd',
    style: 'h4',
    text: 'Reset password',
    tips: {
      id: 'resetPwdTips',
      style: 'font-weight-normal',
      message: `Enter the email address associated with your account, 
      and we’ll email you a link to reset your password.`,
    },
  },
  elements: [
    {
      element: 'input',
      id: 'email',
      label: 'Email address',
      type: 'email',
      placeholder: ''
    }
  ],
  button: {
    id: 'forgetPasswordBtn',
    text: 'Send Reset Link',
    style: 'btn btn-danger btn-block font-weight-bold'
  },
  extraLink: {
    id: 'backToSignInBtn',
    target: '/signIn',
    text: '< Back to Sign In',
  }
}

export {formMeta, formValidation}