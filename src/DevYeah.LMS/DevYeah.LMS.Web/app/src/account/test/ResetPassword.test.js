import React from 'react';
import 'jest-dom/extend-expect';
import { render, fireEvent, cleanup } from '@testing-library/react';
import ResetPassword from '../ResetPassword';

afterEach(cleanup);

const setup = () => {
  const result = render(
    <ResetPassword />
  );
  return result;
};

it('render without crashing', () => {
  const { getByLabelText } = setup();
  const node = getByLabelText('Confirm Password');
  expect(node.id).toBe('confirmPassword');
});

it('activate validation with blur event', async () => {
  const { getByLabelText, findAllByText } = setup();
  const node = getByLabelText('Password');
  fireEvent.blur(node);
  expect(findAllByText('Password confirm is required!')).not.toBeNull();
});