import React from 'react';
import 'jest-dom/extend-expect';
import { render, fireEvent, cleanup } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import EditProfileLayout from '../EditProfileLayout';

afterEach(cleanup);

const setup = () => {
  const result = render(
    <MemoryRouter>
      <EditProfileLayout />
    </MemoryRouter>
  );
  return result;
};

it('render EditProfileLayout without crashing', () => {
  const { getByText } = setup();
  const menuNode = getByText('View Profile');
  expect(menuNode.id).toBe('viewProfileBtn');
});

it('route to photo edit without crashing', async () => {
  const { getByText, findAllByText } = setup();
  const menuNode = getByText('Photo');
  fireEvent.click(menuNode);
  expect(await findAllByText('Upload a file from your computer')).not.toBeNull();
});