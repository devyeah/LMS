import React from 'react';

export default function CardHeader({ header }) {
  return (
    <div className={header.alignStyle}>
      <h1 id={header.id} className={header.style}>{header.text}</h1>
      {header.tips && <p className={header.tips.style} id={header.tips.id}>{header.tips.message}</p>}
    </div>
  );
}
