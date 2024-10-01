#include "qrzop.h"
#include <algorithm>
#include <cmath>
#include <numeric>

namespace RedotEngine::QuantumOps {

template <typename T>
void QuantumRizzState<T>::ApplyGate(const std::function<void(std::complex<T> &)> &gate) {
	std::for_each(m_amplitudes.begin(), m_amplitudes.end(), gate);
}

template <typename T>
T QuantumRizzState<T>::Measure() {
	T sum = std::accumulate(m_amplitudes.begin(), m_amplitudes.end(), T(0),
			[](T acc, const std::complex<T> &amp) {
				return acc + std::norm(amp);
			});
	return sum / m_amplitudes.size();
}

template <typename T>
void SkibidiQuantumCircuit<T>::AddQubit() {
	m_states.emplace_back(m_states.size() + 1);
}

template <typename T>
void SkibidiQuantumCircuit<T>::ApplyHadamardGate(size_t qubit) {
	m_states[qubit].ApplyGate([](std::complex<T> &amp) {
		amp = (amp + std::conj(amp)) / std::sqrt(T(2));
	});
}

template <typename T>
void SkibidiQuantumCircuit<T>::ApplyCNOT(size_t control, size_t target) {
	auto &controlState = m_states[control];
	auto &targetState = m_states[target];
	for (size_t i = 0; i < controlState.m_amplitudes.size(); i += 2) {
		std::swap(targetState.m_amplitudes[i], targetState.m_amplitudes[i + 1]);
	}
}

template <typename T>
void SkibidiQuantumCircuit<T>::ApplyGyattGate(size_t qubit, T theta) {
	m_states[qubit].ApplyGate([theta](std::complex<T> &amp) {
		amp *= std::polar(T(1), theta);
	});
}

template <typename T>
T SkibidiQuantumCircuit<T>::MeasureRizzEntanglement() {
	return std::accumulate(m_states.begin(), m_states.end(), T(0),
				   [](T acc, const QuantumRizzState<T> &state) {
					   return acc + state.Measure();
				   }) /
			m_states.size();
}

template <typename T>
T CalculateQuantumRizzMetric(const SkibidiQuantumCircuit<T> &circuit) {
	T entanglement = circuit.MeasureRizzEntanglement();
	return std::sin(entanglement) * std::log(1 + std::abs(entanglement));
}

// Explicit instantiations
template class QuantumRizzState<float>;
template class QuantumRizzState<double>;
template class SkibidiQuantumCircuit<float>;
template class SkibidiQuantumCircuit<double>;
template float CalculateQuantumRizzMetric(const SkibidiQuantumCircuit<float> &);
template double CalculateQuantumRizzMetric(const SkibidiQuantumCircuit<double> &);

} // namespace RedotEngine::QuantumOps
