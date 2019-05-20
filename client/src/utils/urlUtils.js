import { trimStart, trimEnd } from "./stringUtils";

export function joinUrlWithRoute(url, route) {
  return trimEnd("/", url) + "/" + trimStart("/", route);
}
