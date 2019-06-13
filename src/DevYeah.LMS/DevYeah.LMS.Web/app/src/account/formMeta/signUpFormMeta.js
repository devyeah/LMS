import * as yup from 'yup';

const formMeta = {
  initialValues: { username: '', email: '', password: '', userType: '' },  
  header: {
    id:'title',
    alignStyle: 'text-center',
    style: 'h2',
    text: 'Dev Yeah!',
    tips: {
      id:'subtitle',
      message:'Start learning with Dev Yeah!'
    }
  },
  elements: [
    {
      element: 'input',
      id: 'username',
      label: 'Username',
      type: 'text',
      placeholder: 'Enter Username'
    },
    {
      element: 'input',
      id: 'email',
      label: 'Email',
      type: 'email',
      placeholder: 'Email address'
    },
    {
      element: 'input',
      id: 'password',
      label: 'Password',
      type: 'password',
      placeholder: 'Password',
    },
  ],
  button: {
    id: 'signupBtn',
    text: 'Sign Up',
    style: 'btn btn-danger btn-block font-weight-bold'
  },
  extraLink: [
    {
      id: 'toSignIn',
      target: '/signin',
      text: 'Back to Sign In',
    },
  ]
}

const formValidation = yup.object().shape({
  username: yup.string()
    .min(3, 'Too Short!')
    .max(50, 'Too Long!')
    .required('Required!'),
  email: yup.string()
    .email('Invalid email address')
    .required('Required!'),
  password: yup.string()
    .min(8, 'Too Short!')
    .max(16, 'Too Long!')
    .required('Required!'),
});

export {formMeta, formValidation}