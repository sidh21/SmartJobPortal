declare module '*.scss' {
  const content: { [className: string]: string };
  export default content;
}

// For side-effect imports (like global styles)
declare module '*.scss' {
  const content: any;
  export default content;
}