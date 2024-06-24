// only needed for debugging to be able to intercept the request with a local
// proxy
import fetch from 'node-fetch';
import { bootstrap } from 'global-agent';
bootstrap();

function getCallerLocation(error) {
  const stack = error.stack.split('\n');
  // skip the error message and the current function
  const caller = stack[2].trim();
  const regex = /at (\w+) \(([^)]+)\)/;
  const match = regex.exec(caller);
  const functionName = match[1];
  const filePathLocation = match[2];
  return { functionName, filePathLocation };
}

async function codeLocationFetch(url, options = {}) {
  const { functionName, filePathLocation } = getCallerLocation(new Error());

  const defaultHeaders = {
    'x-src-method': functionName,
    'x-src': filePathLocation
  };

  const mergedOptions = {
    ...options,
    headers: {
      ...defaultHeaders,
      ...options.headers,
    },
  };

  return fetch(url, mergedOptions);
}

async function func1() {
  const res = await codeLocationFetch('https://jsonplaceholder.typicode.com/posts');
  const json = await res.json();
  console.log(json);
}

async function func2() {
  const res = await codeLocationFetch('https://jsonplaceholder.typicode.com/posts/1');
  const json = await res.json();
  console.log(json);
}

await func1();
await func2();