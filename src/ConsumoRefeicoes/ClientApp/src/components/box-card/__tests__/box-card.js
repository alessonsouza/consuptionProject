import React from 'react';
import { render, screen } from '@testing-library/react';
import Box from '../box';
import BoxTitle from '../box-title';

describe('Box Component Tests', () => {
  test('Render a Box', () => {
    const { container } = render(<Box>Teste</Box>);
    const boxContent = screen.getByText('Teste');

    expect(boxContent).toBeInTheDocument();
    expect(container.firstChild).toHaveClass('box');
  });

  test('Render a Box Title', () => {
    const { getByText } = render(<BoxTitle>Teste</BoxTitle>);
    const titleContent = getByText('Teste');
    expect(titleContent).toBeInTheDocument();
  });

  test('Render a box with tittle', () => {
    const { getByText } = render(
      <Box>
        <BoxTitle>Test tittle</BoxTitle>
        Test Content
      </Box>,
    );

    expect(getByText('Test tittle')).toBeInTheDocument();
    expect(getByText('Test Content')).toBeInTheDocument();
  });
});
