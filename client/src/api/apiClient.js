import { toast } from "react-toastify";

export const BASE_URL = process.env.REACT_APP_API_URL + "api";

export function post(url, body) {
  return fetchWrapper(url, "POST", body);
}

export function put(url, body) {
  return fetchWrapper(url, "PUT", body);
}

export function get(url) {
  return fetchWrapper(url, "GET");
}

function fetchWrapper(url, method, body) {
  return fetch(url, {
    method,
    headers: {
      Accept: "application/json",
      Authorization: getAccessToken(),
      "Content-Type": "application/json"
    },
    body: body && JSON.stringify(body)
  })
    .then(handleResponse)
    .catch(handleError);
}

async function handleResponse(response) {
  const responseToJson = await response.json();
  if (response.ok) {
    return responseToJson;
  } else {
    throw responseToJson;
  }
}

function handleError(error) {
  console.error(JSON.stringify(error));
  toast.error(error.messages.join(", "));
  throw error;
}

function getAccessToken() {
  const token = localStorage.getItem("access_token");
  return token && `Bearer ${token}`;
}
