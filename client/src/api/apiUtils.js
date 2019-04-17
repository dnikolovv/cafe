export async function handleResponse(response) {
  if (response.ok) {
    return response.json();
  } else {
    const error = await response.json();
    throw error;
  }
}

// In a real app, would likely call an error logging service.
export function handleError(error) {
  // eslint-disable-next-line no-console
  console.error(JSON.stringify(error));
  throw error;
}
