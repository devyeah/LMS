import React from 'react';
import 'jest-dom/extend-expect';
import { MemoryRouter } from 'react-router-dom';
import Navbar from '../components/Navbar';
import { render } from '@testing-library/react';

test('render the Navbar', () => {
  const { getByText } = render(
    <MemoryRouter>
      <Navbar />
    </MemoryRouter>
  );
  const signUpBtnNode = getByText("Sign Up");
  const signInBtnNode = getByText('Sign In');
  expect(signUpBtnNode.className).toBe('btn btn-success');
  expect(signInBtnNode.className).toBe('btn btn-outline-primary');
})