import React from 'react';
import 'jest-dom/extend-expect';
import { MemoryRouter } from 'react-router-dom';
import Navbar from '../Navbar';
import { render } from '@testing-library/react';

test('render the Navbar', () => {
  const { getByText } = render(
    <MemoryRouter>
      <Navbar />
    </MemoryRouter>
  );
  const signUpBtnNode = getByText("Sign Up");
  const signInBtnNode = getByText('Sign In');
  expect(signUpBtnNode.className).toBe('btn btn-success font-weight-bold');
  expect(signInBtnNode.className).toBe('btn btn-outline-primary font-weight-bold');
})